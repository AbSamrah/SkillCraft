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
        Task<Quiz> GetById(string id);
        Task<List<Quiz>> GetAll();
        Task Update(Quiz quiz);
        void Remove(string id);
    }
}
