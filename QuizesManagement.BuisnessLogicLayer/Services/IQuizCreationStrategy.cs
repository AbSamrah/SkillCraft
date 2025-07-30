using QuizesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public interface IQuizCreationStrategy
    {
        Task<MultipleChoicesQuiz> CreateMultipleChoicesQuiz(object parameters);

        Task<TrueOrFalseQuiz> CreateTrueOrFalseQuiz(object parameters); 

    }
}
