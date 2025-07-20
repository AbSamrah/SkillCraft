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
    public class MilestoneRepository :  BaseRepository<Milestone>, IMilestoneRepository
    {
        public MilestoneRepository(IRoadmapDbContext context) : base(context)
        {
        }

        public override async Task<Milestone> GetById(string milestoneId)
        {
            // Validate ObjectId format first
            if (!ObjectId.TryParse(milestoneId, out _))
            {
                return null;
            }

            var filter = Builders<Milestone>.Filter.Eq("_id", ObjectId.Parse(milestoneId));
            var data = await _dbSet.FindAsync(filter);
            var milestone = data.SingleOrDefault();
            if (milestone == null || milestone.StepsIds?.Any() != true)
                return milestone;

            var stepObjectIds = new List<ObjectId>();
            foreach (var id in milestone.StepsIds)
            {
                if (ObjectId.TryParse(id, out var stepObjectId))
                {
                    stepObjectIds.Add(stepObjectId);
                }
            }

            // Create filter using FieldDefinition instead of lambda
            var field = new StringFieldDefinition<Step, ObjectId>("_id");
            var stepFilter = Builders<Step>.Filter.In(field, stepObjectIds);

            var steps = await _context.GetCollection<Step>("Step")
                .Find(stepFilter)
                .ToListAsync();

            milestone.Steps = steps;
            return milestone;

        }

    }
}
