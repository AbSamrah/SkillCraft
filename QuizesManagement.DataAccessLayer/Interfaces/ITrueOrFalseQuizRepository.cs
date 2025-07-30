using QuizesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.DataAccessLayer.Interfaces
{
    public interface ITrueOrFalseQuizRepository : IQuizRepository
    {
        Task<TrueOrFalseQuiz> GetById(string id);

        Task Update(TrueOrFalseQuiz quiz);
    }
}
