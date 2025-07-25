using MongoDB.Bson;
using MongoDB.Driver;
using RoadmapMangement.DataAccessLayer.Interfaces;
using RoadmapMangement.DataAccessLayer.Models;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RoadmapMangement.DataAccessLayer.Repositories
{
    public class MilestoneRepository :  BaseRepository<Milestone>, IMilestoneRepository
    {
        public MilestoneRepository(IRoadmapDbContext context) : base(context)
        {
        }

        public override async Task<Milestone> GetById(string milestoneId)
        {
            var milestone = await _dbSet.Find(m => m.Id == milestoneId).FirstOrDefaultAsync();
            if (milestone == null || milestone.StepsIds?.Any() != true)
                return milestone;

            var unorderedSteps = await _context.GetCollection<Step>("Step")
                .Find(s => milestone.StepsIds.Contains(s.Id))
                .ToListAsync();

            var stepDict = unorderedSteps.ToDictionary(s => s.Id);
            milestone.Steps = milestone.StepsIds
                                       .Where(id => stepDict.ContainsKey(id))
                                       .Select(id => stepDict[id])
                                       .ToList();

            milestone.DurationInMinutes = milestone.Steps.Sum(s => s.DurationInMinutes);

            return milestone;
        }

    }
}
