using QuizesManagement.BuisnessLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public interface IMultipleChoisesQuizService: IQuizService
    {
        public Task<MultipleChoicesQuizDto> GetById(string id);

        public Task<MultipleChoicesQuizDto> Update(EditMultipleChoicesQuizRequest quizRequest);

        Task<MultipleChoicesQuizDto> Add(IQuizCreationStrategy strategy, object parameters);

        public Task<bool> CheckAnswer(string quizId, string answer);
    }
}
