using AutoMapper;
using Moq;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.BuisnessLogicLayer.Services;
using RoadmapMangement.DataAccessLayer.Interfaces;
using RoadmapMangement.DataAccessLayer.Models;
using System.Threading.Tasks;
using Xunit;

namespace RoadmapMangement.BuisnessLogicLayer.Tests
{
    public class RoadmapsServiceTests
    {
        private readonly Mock<IRoadmapRepository> _roadmapRepositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RoadmapsService _roadmapsService;

        public RoadmapsServiceTests()
        {
            _roadmapRepositoryMock = new Mock<IRoadmapRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _roadmapsService = new RoadmapsService(
                _roadmapRepositoryMock.Object,
                _uowMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Get_ShouldReturnRoadmapDto_WhenRoadmapExists()
        {
            // Arrange
            var roadmapId = "test-id";
            var roadmap = new Roadmap { Id = roadmapId, Name = "Test Roadmap" };
            var roadmapDto = new RoadmapDto { Id = roadmapId, Name = "Test Roadmap" };

            _roadmapRepositoryMock.Setup(r => r.GetById(roadmapId)).ReturnsAsync(roadmap);
            _mapperMock.Setup(m => m.Map<RoadmapDto>(roadmap)).Returns(roadmapDto);

            // Act
            var result = await _roadmapsService.Get(roadmapId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(roadmapId, result.Id);
            _roadmapRepositoryMock.Verify(r => r.GetById(roadmapId), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldCallRepositoryAddAndCommit_WhenCalled()
        {
            // Arrange
            var addRoadmapRequest = new AddRoadmapRequest { Name = "New Roadmap" };
            var roadmap = new Roadmap { Name = "New Roadmap" };
            var roadmapDto = new RoadmapDto { Name = "New Roadmap" };
            var strategyMock = new Mock<IRoadmapCreationStrategy>();
            strategyMock.Setup(s => s.CreateRoadmap(addRoadmapRequest)).ReturnsAsync(roadmap);

            _mapperMock.Setup(m => m.Map<RoadmapDto>(roadmap)).Returns(roadmapDto);

            // Act
            var result = await _roadmapsService.Add(strategyMock.Object, addRoadmapRequest);
            
            // Assert
            Assert.NotNull(result);
            _roadmapRepositoryMock.Verify(r => r.Add(It.IsAny<Roadmap>()), Times.Once);
            _uowMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}
