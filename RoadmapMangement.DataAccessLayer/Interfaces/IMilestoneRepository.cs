using RoadmapMangement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.DataAccessLayer.Interfaces
{
    public interface   IMilestoneRepository: IRepository<Milestone>
    {
        //public Task<Milestone> GetMilestoneWithSteps(string milestoneId);
        //public  Task<bool> AddStepsToMilestone(string milestoneId, List<string> stepIds);
    }
}
