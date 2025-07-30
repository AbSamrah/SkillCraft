using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public class StrategyFactory : IStrategyFactory
    {
        private readonly AiQuizCreationStrategy _aiStrategy;
        private readonly ManualQuizCreationStrategy _manualStrategy;

        public StrategyFactory(AiQuizCreationStrategy aiStrategy, ManualQuizCreationStrategy manualStrategy)
        {
            _aiStrategy = aiStrategy;
            _manualStrategy = manualStrategy;
        }

        public IQuizCreationStrategy CreateStrategy(string key)
        {
            switch (key.ToLowerInvariant())
            {
                case "ai":
                    return _aiStrategy;
                case "manual":
                    return _manualStrategy;
                default:
                    throw new KeyNotFoundException($"Strategy '{key}' not found.");
            }
        }
    }
}
