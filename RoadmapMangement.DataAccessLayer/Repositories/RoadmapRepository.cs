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
        public RoadmapRepository(IMongoContext context) : base(context)
        {
        }

        public async Task<List<Roadmap>> GetActiveRoadmaps()
        {
            var filter = Builders<Roadmap>.Filter.Eq(x => x.IsActive, true);
            var result = await _dbSet.FindAsync(filter);
            return await result.ToListAsync();
        }

        //public async Task<bool> AddMilestoneToRoadmap(string roadmapId, Milestone milestone)
        //{
        //    var roadmapFilter = Builders<Roadmap>.Filter.Eq(x => x.Id, roadmapId);
        //    var update = Builders<Roadmap>.Update
        //        .Push(x => x.MilestoneIds, milestone.Id);

        //    var result = await _dbSet.UpdateOneAsync(roadmapFilter, update);
        //    return result.ModifiedCount > 0;
        //}

        public override async Task<Roadmap> GetById(string roadmapId)
        {
            var roadmap = await _dbSet.Find(r => r.Id == roadmapId).FirstOrDefaultAsync();
            if (roadmap == null || roadmap.MilestoneIds?.Any() != true)
                return roadmap;

            var milestones = await _context.GetCollection<Milestone>("Milestone")
                .Find(m => roadmap.MilestoneIds.Contains(m.Id))
                .ToListAsync();

            roadmap.Milestones = milestones;
            return roadmap;
        }
    }

}
