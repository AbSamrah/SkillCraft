using MongoDB.Bson;
using MongoDB.Driver;
using RoadmapMangement.DataAccessLayer.Interfaces;
using RoadmapMangement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.DataAccessLayer.Repositories
{
    public class RoadmapRepository : BaseRepository<Roadmap>, IRoadmapRepository
    {
        public RoadmapRepository(IRoadmapDbContext context) : base(context)
        {
        }

        public async Task<List<Roadmap>> GetActiveRoadmaps()
        {
            var filter = Builders<Roadmap>.Filter.Eq(x => x.IsActive, true);
            var result = await _dbSet.FindAsync(filter);
            return await result.ToListAsync();
        }


        public override async Task<Roadmap> GetById(string roadmapId)
        {
            var roadmap = await _dbSet.Find(r => r.Id == roadmapId).FirstOrDefaultAsync();
            if (roadmap == null || roadmap.MilestonesIds?.Any() != true)
                return roadmap;

            var unorderedMilestones = await _context.GetCollection<Milestone>("Milestone")
                .Find(m => roadmap.MilestonesIds.Contains(m.Id))
                .ToListAsync();

            var milestoneDict = unorderedMilestones.ToDictionary(m => m.Id);
            var orderedMilestones = roadmap.MilestonesIds
                                          .Where(id => milestoneDict.ContainsKey(id))
                                          .Select(id => milestoneDict[id])
                                          .ToList();

            var allStepIds = orderedMilestones.SelectMany(m => m.StepsIds ?? new List<string>()).Distinct().ToList();

            if (allStepIds.Any())
            {
                var allSteps = await _context.GetCollection<Step>("Step")
                    .Find(s => allStepIds.Contains(s.Id))
                    .ToListAsync();
                var stepDict = allSteps.ToDictionary(s => s.Id);

                foreach (var milestone in orderedMilestones)
                {
                    if (milestone.StepsIds != null)
                    {
                        milestone.Steps = milestone.StepsIds
                                                   .Where(id => stepDict.ContainsKey(id))
                                                   .Select(id => stepDict[id])
                                                   .ToList();
                        
                        // Calculate duration for this milestone
                        milestone.DurationInMinutes = milestone.Steps.Sum(s => s.DurationInMinutes);
                    }
                }
            }
            
            roadmap.Milestones = orderedMilestones;
            // Calculate total duration for the roadmap
            roadmap.DurationInMinutes = roadmap.Milestones.Sum(m => m.DurationInMinutes);

            return roadmap;
        }
    }
}
