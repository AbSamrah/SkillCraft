using AutoMapper;
using Moq;
using ProfilesManagement.BuisnessLogicLayer.Services;
using ProfilesManagement.DataAccessLayer.Interfaces;
using ProfilesManagement.DataAccessLayer.Models;
using System.Threading.Tasks;
using Xunit;

namespace ProfilesManagement.BuisnessLogicLayer.Tests
{
    public class ProfileServiceTests
    {
        private readonly Mock<IProfileRepository> _profileRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProfileService _profileService;

        public ProfileServiceTests()
        {
            _profileRepositoryMock = new Mock<IProfileRepository>();
            _mapperMock = new Mock<IMapper>();
            _profileService = new ProfileService(
                _profileRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Add_ShouldCallRepositoryAdd_WhenCalled()
        {
            // Arrange
            var userId = "test-user-id";

            // Act
            await _profileService.Add(userId);

            // Assert
            _profileRepositoryMock.Verify(r => r.Add(It.Is<UserProfile>(p => p.Id == userId)), Times.Once);
        }

        [Fact]
        public async Task AddRoadmap_ShouldUpdateProfile_WhenRoadmapIsNotInProfile()
        {
            // Arrange
            var userId = "test-user-id";
            var roadmapId = "test-roadmap-id";
            var profile = new UserProfile(userId);

            _profileRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(profile);

            // Act
            await _profileService.AddRoadmap(userId, roadmapId);

            // Assert
            Assert.Contains(profile.Roadmaps, r => r.Id == roadmapId);
            _profileRepositoryMock.Verify(r => r.Update(profile), Times.Once);
        }
    }
}
