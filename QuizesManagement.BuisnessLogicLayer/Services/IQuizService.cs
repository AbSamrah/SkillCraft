using QuizesManagement.BuisnessLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public interface IQuizService
    {
        public Task<List<QuizDto>> GetAllQuizes();

        public Task<QuizDto> Get(string id);

        public Task<QuizDto> Add(QuizRequest quizRequest);

        public Task<QuizDto> Update(QuizRequest quizRequest);

        public Task<QuizDto> Delete(string id);

        public Task<bool> CheckAnswer(QuizDto quiz, IAnswer answer);
    }
}
