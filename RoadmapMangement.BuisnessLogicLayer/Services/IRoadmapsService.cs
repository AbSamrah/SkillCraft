using RoadmapMangement.BuisnessLogicLayer.Filters;
using RoadmapMangement.BuisnessLogicLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Services
{
    public interface IRoadmapsService
    {
        Task<List<RoadmapDto>> GetAll(EntityFilter filter);

        Task<RoadmapDto> Add(IRoadmapCreationStrategy strategy, object parameters);

        Task<RoadmapDto> Get(string id);

        Task<RoadmapDto> DeleteAsync(string id);

        Task<RoadmapDto> UpdateAsync(string id, UpdateRoadmapRequest updateRoadmapRequest);
    }
}
