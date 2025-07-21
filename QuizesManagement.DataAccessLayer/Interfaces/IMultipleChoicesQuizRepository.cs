using QuizesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.DataAccessLayer.Interfaces
{
    public interface IMultipleChoicesQuizRepository: IQuizRepository
    {
        Task<MultipleChoicesQuiz> GetById(string id);

        Task Update(MultipleChoicesQuiz quiz);
    }
}
