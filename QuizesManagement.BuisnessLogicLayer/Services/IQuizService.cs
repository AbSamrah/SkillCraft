using QuizesManagement.BuisnessLogicLayer.Filters;
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
        public Task<List<QuizDto>> GetAll(QuizFilter filter);

        public Task<QuizDto> Delete(string id);

    }
}
