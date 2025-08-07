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
using UsersManagement.DataAccessLayer.Repositories;
using UsersManagement.BuissnessLogicLayer.Services;

namespace UsersManagement.BuisnessLogicLayer.Tests
{

    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPendingUserRepository> _pendingUserRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IEmailService> _emailService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _pendingUserRepositoryMock = new Mock<IPendingUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _emailService = new Mock<IEmailService>();
            _mapperMock = new Mock<IMapper>();
            _authService = new AuthService(
                _userRepositoryMock.Object,
                _pendingUserRepositoryMock.Object,
                _passwordHasherMock.Object,
                _roleRepositoryMock.Object,
                _mapperMock.Object,
                _emailService.Object
            );
        }

        [Fact]
        public async Task SignUpAsync_ShouldThrowException_WhenEmailAlreadyExists()
        {
            var userSignUp = new UserSignUp { Email = "test@example.com" };
            _userRepositoryMock.Setup(r => r.UserExistsAsync(userSignUp.Email)).ReturnsAsync(true);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _authService.SignUpAsync(userSignUp));
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

            await _authService.SignUpAsync(userSignUp);

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
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _authService.LoginAsync(userLogin));
            Assert.Equal("Password is incorrect.", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenEmailDoesNotExist()
        {
            // Arrange
            var userLogin = new UserLogin { Email = "notexist@example.com", Password = "hashed_correct_password" };
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "exist@example.com",
                PasswordHash = "hashed_correct_password"
            };
            // Simulate when the email does not exist
            _userRepositoryMock.Setup(r => r.UserExistsAsync(userLogin.Email)).ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _authService.LoginAsync(userLogin));
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnUserDto_WhenLoginSuccess()
        {
            // Arrange
            var userLogin = new UserLogin { Email = "exist@example.com", Password = "hashed_correct_password" };
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "exist@example.com",
                PasswordHash = "hashed_correct_password"
            };
            var userDto = new UserDto
            {
                Id = Guid.NewGuid(),
                Email = "exist@example.com"
            };

            _userRepositoryMock.Setup(r => r.UserExistsAsync(userLogin.Email)).ReturnsAsync(true);
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(userLogin.Email)).ReturnsAsync(existingUser);
            _mapperMock.Setup(m => m.Map<UserDto>(existingUser)).Returns(userDto);
            // Simulate the login done successfuly
            _passwordHasherMock.Setup(h => h.Verify(userLogin.Password, existingUser.PasswordHash)).Returns(true);

            var result = await _authService.LoginAsync(userLogin);

            // Act & Assert
            Assert.NotNull(result);
            Assert.Equal(userLogin.Email, result.Email);
            _userRepositoryMock.Verify(r => r.GetByEmailAsync(userLogin.Email), Times.Once);
        }
    }
}
