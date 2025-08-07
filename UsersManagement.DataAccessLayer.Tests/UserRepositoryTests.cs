using DataAccessLayer.Auth;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace UsersManagement.DataAccessLayer.Tests
{
    public class UserRepositoryTests
    {
        private readonly UsersDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<UsersDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new UsersDbContext(options);
            _passwordHasher = new PasswordHasher();
            _userRepository = new UserRepository(_context);
        }

        [Fact]
        public async Task AddAsync_And_GetByEmailAsync_ShouldWorkCorrectly()
        {
            // Arrange
            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                PasswordHash = _passwordHasher.Hash("password123"),
                RoleId = Guid.NewGuid()
            };

            // Act
            await _userRepository.AddAsync(user);
            var retrievedUser = await _userRepository.GetByEmailAsync(user.Email);

            // Assert
            Assert.NotNull(retrievedUser);
            Assert.Equal(user.Email, retrievedUser.Email);
        }
    }
}