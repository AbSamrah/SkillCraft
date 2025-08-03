using Moq;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using DataAccessLayer.Auth;
using Xunit;
using System;
using BuissnessLogicLayer.Models;
using BuissnessLogicLayer.Services;
using ProfilesManagement.BuisnessLogicLayer.Services;
using AutoMapper;

namespace UsersManagement.BuisnessLogicLayer.Tests
{

    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _mapperMock = new Mock<IMapper>();
            _authService = new AuthService(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _roleRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task SignUpAsync_ShouldThrowException_WhenEmailAlreadyExists()
        {
            var userSignUp = new UserSignUp { Email = "test@example.com" };
            _userRepositoryMock.Setup(r => r.UserExistsAsync(userSignUp.Email)).ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => _authService.SignUpAsync(userSignUp));
        }

        [Fact]
        public async Task SignUpAsync_ShouldReturnUserDto_WhenSignUpIsSuccessful()
        {
            var userSignUp = new UserSignUp
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = "password123"
            };
            var userDto = new UserDto { Email = userSignUp.Email };

            _userRepositoryMock.Setup(r => r.UserExistsAsync(userSignUp.Email)).ReturnsAsync(false);
            _roleRepositoryMock.Setup(r => r.GetByTitleAsync("User")).ReturnsAsync(new Role());
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

            var result = await _authService.SignUpAsync(userSignUp);

            Assert.NotNull(result);
            Assert.Equal(userSignUp.Email, result.Email);
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var userLogin = new UserLogin { Email = "test@example.com", Password = "wrongpassword" };
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                PasswordHash = "hashed_correct_password"
            };

            _userRepositoryMock.Setup(r => r.UserExistsAsync(userLogin.Email)).ReturnsAsync(true);
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(userLogin.Email)).ReturnsAsync(existingUser);

            // Simulate the password verifier returning false (password doesn't match)
            _passwordHasherMock.Setup(h => h.Verify(userLogin.Password, existingUser.PasswordHash)).Returns(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(userLogin));
            Assert.Equal("Password is incorrect.", exception.Message);
        }
    }
}
