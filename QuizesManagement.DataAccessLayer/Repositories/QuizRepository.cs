using MongoDB.Bson;
using MongoDB.Driver;
using QuizesManagement.DataAccessLayer.Interfaces;
using QuizesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.DataAccessLayer.Repositories
{
    public abstract class QuizRepository: IQuizRepository
    {
        protected readonly IQuizDbContext _context;
        protected IMongoCollection<Quiz> _dbSet;

        public QuizRepository(IQuizDbContext context)
        {
            _context = context;

            _dbSet = _context.GetCollection<Quiz>(typeof(Quiz).Name);
        }

        public virtual void Add(Quiz quiz)
        {
            if (string.IsNullOrEmpty(quiz.Id))
            {
                quiz.Id = ObjectId.GenerateNewId().ToString();
            }
            else if (!ObjectId.TryParse(quiz.Id, out _))
            {
                throw new ArgumentException("ID must be a valid 24-character hex string");
            }

            _context.AddCommand(() => _dbSet.InsertOneAsync(quiz));
        }


        public virtual async Task<Quiz> GetById(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            var filter = Builders<Quiz>.Filter.Eq("_id", ObjectId.Parse(id));
            var data = await _dbSet.FindAsync(filter);
            var entity = data.SingleOrDefault();

            return entity;
        }

        public virtual async Task<List<Quiz>> GetAll()
        {
            var all = await _dbSet.FindAsync(Builders<Quiz>.Filter.Empty);
            return all.ToList();
        }

        public virtual async Task Update(Quiz quiz)
        {
            if (!ObjectId.TryParse(quiz.Id, out var objectId))
            {
                throw new ArgumentException("Invalid ID format");
            }

            _context.AddCommand(async () =>
            {
                var filter = Builders<Quiz>.Filter.Eq("_id", objectId);
                var result = await _dbSet.ReplaceOneAsync(filter, quiz);
            });

        }

        public virtual void Remove(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ID format");
            }

            _context.AddCommand(() => _dbSet.DeleteOneAsync(
                Builders<Quiz>.Filter.Eq("_id", objectId)
            ));
        }



        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
