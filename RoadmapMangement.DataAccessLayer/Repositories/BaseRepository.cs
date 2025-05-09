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
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly IMongoContext _context;
        protected IMongoCollection<TEntity> _dbSet;

        public BaseRepository(IMongoContext context)
        {
            _context = context;

            _dbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual void Add(TEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Id) || !ObjectId.TryParse(entity.Id, out _))
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
            }

            _context.AddCommand(() => _dbSet.InsertOneAsync(entity));
        }

        public virtual async Task<TEntity> GetById(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            var data = await _dbSet.FindAsync(filter);
            return await data.SingleOrDefaultAsync();
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            var all = await _dbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual void Update(TEntity entity)
        {
            _context.AddCommand(() => _dbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity));
        }

        public virtual void Remove(string id)
        {
            _context.AddCommand(() => _dbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
