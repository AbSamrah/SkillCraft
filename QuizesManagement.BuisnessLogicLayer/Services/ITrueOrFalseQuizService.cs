using QuizesManagement.BuisnessLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public interface ITrueOrFalseQuizService : IQuizService
    {
        Task<TrueOrFalseQuizDto> GetById(string id);

        Task<TrueOrFalseQuizDto> Add(IQuizCreationStrategy strategy, object parameters);

        Task<TrueOrFalseQuizDto> Update(EditTrueOrFalseQuizRequest request);

        Task<bool> CheckAnswer(string quizId, bool answer);

    }
}
