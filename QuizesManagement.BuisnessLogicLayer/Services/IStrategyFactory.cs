using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public interface IStrategyFactory
    {
        IQuizCreationStrategy CreateStrategy(string key);
    }
}
