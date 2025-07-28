using RoadmapMangement.BuisnessLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Services
{
    public interface IRoadmapsService
    {
        Task<List<RoadmapDto>> GetAll();

        Task<RoadmapDto> Add(AddRoadmapRequest addRoadmapRequest);

        Task<RoadmapDto> Get(string id);

        Task<RoadmapDto> DeleteAsync(string id);

        Task<RoadmapDto> UpdateAsync(string id, UpdateRoadmapRequest updateRoadmapRequest);
    }
}
