using RoadmapMangement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.DataAccessLayer.Interfaces
{
    public interface IRoadmapRepository : IRepository<Roadmap>
    {
        Task<List<Roadmap>> GetActiveRoadmaps();
        //Task<bool> AddMilestoneToRoadmap(string roadmapId, Milestone milestone);
        //Task<Roadmap> GetRoadmapWithMilestones(string roadmapId);
    }
}
