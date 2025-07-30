﻿using MongoDB.Bson;
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
        protected readonly IRoadmapDbContext _context;
        protected IMongoCollection<TEntity> _dbSet;

        public BaseRepository(IRoadmapDbContext context)
        {
            _context = context;

            _dbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual void Add(TEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
            }
            else if (!ObjectId.TryParse(entity.Id, out _))
            {
                throw new ArgumentException("ID must be a valid 24-character hex string");
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
            var entity = data.SingleOrDefault();

            return entity;
        }

        public virtual async Task<List<TEntity>> GetAll(string name = "", int pageNumber = 0, int pageSize = 9)
        {
            var filter = string.IsNullOrEmpty(name)
                ? Builders<TEntity>.Filter.Empty
                : Builders<TEntity>.Filter.Regex("Name", new MongoDB.Bson.BsonRegularExpression(name, "i"));

            return await _dbSet.Find(filter)
                               .Skip(pageNumber * pageSize)
                               .Limit(pageSize)
                               .ToListAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
            if (!ObjectId.TryParse(entity.Id, out var objectId))
            {
                throw new ArgumentException("Invalid ID format");
            }

            _context.AddCommand(async () =>
            {
                var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
                var result = await _dbSet.ReplaceOneAsync(filter, entity);
            });

        }

        public virtual void Remove(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ID format");
            }

            _context.AddCommand(() => _dbSet.DeleteOneAsync(
                Builders<TEntity>.Filter.Eq("_id", objectId)
            ));
        }

        

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
