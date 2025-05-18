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
    public class MilestoneRepository :  BaseRepository<Milestone>, IMilestoneRepository
    {
        public MilestoneRepository(IMongoContext context) : base(context)
        {
        }

        //public async Task<bool> AddStepToMilestone(string milestoneId, Step step)
        //{
        //    var milestoneFilter = Builders<Milestone>.Filter.Eq(x => x.Id, milestoneId);
        //    var update = Builders<Milestone>.Update
        //        .Push(x => x.StepIds, step.Id);

        //    var result = await _dbSet.UpdateOneAsync(milestoneFilter, update);
        //    return result.ModifiedCount > 0;
        //}

        public override async Task<Milestone> GetById(string milestoneId)
        {
            var milestone = await _dbSet.Find(m => m.Id == milestoneId).FirstOrDefaultAsync();
            if (milestone == null || milestone.StepsIds?.Any() != true)
                return milestone;

            var steps = await _context.GetCollection<Step>("Step")
                .Find(s => milestone.StepsIds.Contains(s.Id))
                .ToListAsync();

            milestone.Steps = steps;
            return milestone;
        }

        //public async Task<bool> AddStepsToMilestone(string milestoneId, List<string> stepIds)
        //{
        //    var milestoneFilter = Builders<Milestone>.Filter.Eq(x => x.Id, milestoneId);
        //    var update = Builders<Milestone>.Update
        //        .PushEach(x => x.StepIds, stepIds);   

        //    var result = await _dbSet.UpdateOneAsync(milestoneFilter, update);
        //    return result.ModifiedCount > 0;
        //}
    }
}
