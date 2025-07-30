using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Services
{
    public class StrategyFactory : IStrategyFactory
    {
        private readonly AiRoadmapCreationStrategy _aiStrategy;
        private readonly ManualRoadmapCreationStrategy _manualStrategy;

        public StrategyFactory(AiRoadmapCreationStrategy aiStrategy, ManualRoadmapCreationStrategy manualStrategy)
        {
            _aiStrategy = aiStrategy;
            _manualStrategy = manualStrategy;
        }

        public IRoadmapCreationStrategy CreateStrategy(string key)
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
