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
    public class TrueOrFalseQuizRepository : QuizRepository, ITrueOrFalseQuizRepository
    {
        public TrueOrFalseQuizRepository(IQuizDbContext context) : base(context)
        {

        }

        public async Task Update(TrueOrFalseQuiz quiz)
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

        public async Task<TrueOrFalseQuiz> GetById(string id)
        {
            var quiz = (TrueOrFalseQuiz)(await _dbSet.Find(r => r.Id == id).FirstOrDefaultAsync());
            if (quiz is null)
            {
                return null;
            }

            return quiz;
        }
    }
}
