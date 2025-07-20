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
            if (roadmap == null || roadmap.MilestoneIds?.Any() != true)
                return roadmap;

            var unorderedMilestones = await _context.GetCollection<Milestone>("Milestone")
                .Find(m => roadmap.MilestoneIds.Contains(m.Id))
                .ToListAsync();

            // Reorder the milestones to match the order in the MilestoneIds list
            var milestoneDict = unorderedMilestones.ToDictionary(m => m.Id);
            var orderedMilestones = roadmap.MilestoneIds
                                          .Where(id => milestoneDict.ContainsKey(id))
                                          .Select(id => milestoneDict[id])
                                          .ToList();

            roadmap.Milestones = orderedMilestones;
            return roadmap;

            //    if (!ObjectId.TryParse(roadmapId, out var objectId))
            //    {
            //        return null;
            //    }

            //    //var roadmap = await _dbSet.Find(r => r.Id == roadmapId).FirstOrDefaultAsync();

            //    var filter = Builders<Roadmap>.Filter.Eq("_id", ObjectId.Parse(roadmapId));
            //    var data = await _dbSet.FindAsync(filter);
            //    var roadmap = data.SingleOrDefault();
            //    if (roadmap == null || roadmap.MilestoneIds?.Any() != true)
            //        return roadmap;

            //    var milestoneObjectIds = new List<ObjectId>();
            //    foreach (var id in roadmap.MilestoneIds)
            //    {
            //        if (ObjectId.TryParse(id, out var milestoneObjectId))
            //        {
            //            milestoneObjectIds.Add(milestoneObjectId);
            //        }
            //    }

            //    // Create filter using FieldDefinition instead of lambda
            //    var field = new StringFieldDefinition<Milestone, ObjectId>("_id");
            //    var milestoneFilter = Builders<Milestone>.Filter.In(field, milestoneObjectIds);

            //    var milestones = await _context.GetCollection<Milestone>("Milestone")
            //        .Find(milestoneFilter)
            //        .ToListAsync();

            //    roadmap.Milestones = milestones;
            //    return roadmap;
            //}
        }
    }
}
