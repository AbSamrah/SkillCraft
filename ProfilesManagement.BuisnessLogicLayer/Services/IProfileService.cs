using ProfilesManagement.BuisnessLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilesManagement.BuisnessLogicLayer.Services
{
    public interface IProfileService
    {
        Task Add(string userId);

        Task<List<RoadmapStatusDto>> GetAllRoadmaps(string userId);

        Task<List<RoadmapStatusDto>> GetRoadmaps(string userId, bool isFinished=true);

        Task<List<string>> GetFinishedStep(string userId, List<string> stepsIds);

        Task FinishSteps(string userId, List<string> stepsIds);

        Task UnFinishSteps(string userId, List<string> stepsIds);

        Task ChangeRoadmapStatus(string userId, string roadmapId, bool finished = true);

        Task AddRoadmap(string  userId, string roadmapId);

        Task RemoveRoadmap(string userId, string roadmapId);

        Task Remove(string userId);
    }
}
