using QuizesManagement.DataAccessLayer.Models;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.DataAccessLayer.Interfaces
{
    public interface IQuizRepository
    {
        void Add(Quiz quiz);
        Task<List<Quiz>> GetAll(List<string> tags, List<string> finishedQuizzes, int pageNumber = 0, int pageSize = 9);
        Task<Quiz> GetById(string id);
        void Remove(string id);
    }
}
