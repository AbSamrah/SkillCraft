using AutoMapper;
using Moq;
using QuizesManagement.BuisnessLogicLayer.Models;
using QuizesManagement.BuisnessLogicLayer.Services;
using QuizesManagement.DataAccessLayer.Interfaces;
using QuizesManagement.DataAccessLayer.Models;
using System.Threading.Tasks;
using Xunit;

namespace QuizesManagement.BuisnessLogicLayer.Tests
{
    public class QuizServiceTests
    {
        private readonly Mock<IQuizRepository> _quizRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly QuizService _quizService;

        public QuizServiceTests()
        {
            _quizRepositoryMock = new Mock<IQuizRepository>();
            _mapperMock = new Mock<IMapper>();
            _uowMock = new Mock<IUnitOfWork>();
            _quizService = new QuizService(
                _quizRepositoryMock.Object,
                _mapperMock.Object,
                _uowMock.Object
            );
        }

        [Fact]
        public async Task Delete_ShouldReturnQuizDto_WhenQuizExists()
        {
            // Arrange
            var quizId = "test-quiz-id";
            var quiz = new Quiz { Id = quizId, Question = "Test Question?" };
            var quizDto = new QuizDto { Id = quizId, Question = "Test Question?" };

            _quizRepositoryMock.Setup(r => r.GetById(quizId)).ReturnsAsync(quiz);
            _mapperMock.Setup(m => m.Map<QuizDto>(quiz)).Returns(quizDto);

            // Act
            var result = await _quizService.Delete(quizId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(quizId, result.Id);
            _quizRepositoryMock.Verify(r => r.Remove(quizId), Times.Once);
            _uowMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}
