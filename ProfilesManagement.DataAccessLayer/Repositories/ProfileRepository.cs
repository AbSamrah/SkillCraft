using MongoDB.Bson;
using MongoDB.Driver;
using ProfilesManagement.DataAccessLayer.Interfaces;
using ProfilesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilesManagement.DataAccessLayer.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IProfileDbContext _context;
        private readonly IMongoCollection<UserProfile> _dbSet;

        public ProfileRepository(IProfileDbContext context)
        {
            _context = context;

            _dbSet = _context.GetCollection<UserProfile>("Profile");
        }
        public async Task Add(UserProfile profile)
        {
            _context.AddCommand(() => _dbSet.InsertOneAsync(profile));
            await _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<List<UserProfile>> GetAll()
        {
            var all = await _dbSet.FindAsync(Builders<UserProfile>.Filter.Empty);
            return all.ToList();
        }

        public async Task<UserProfile> GetById(string id)
        {
            var filter = Builders<UserProfile>.Filter.Eq(p => p.Id, id);
            var data = await _dbSet.FindAsync(filter);
            return await data.SingleOrDefaultAsync();
        }

        public async Task Remove(string id)
        {
            var filter = Builders<UserProfile>.Filter.Eq(p => p.Id, id);
            _context.AddCommand(() => _dbSet.DeleteOneAsync(filter));
            await _context.SaveChanges();
        }

        public async Task Update(UserProfile profile)
        {
            var filter = Builders<UserProfile>.Filter.Eq(p => p.Id, profile.Id);
            _context.AddCommand(() => _dbSet.ReplaceOneAsync(filter, profile));
            await _context.SaveChanges();
        }
    }
}
