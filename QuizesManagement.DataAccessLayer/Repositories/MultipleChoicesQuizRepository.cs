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
    public class MultipleChoicesQuizRepository : QuizRepository, IMultipleChoicesQuizRepository
    {
        public MultipleChoicesQuizRepository(IQuizDbContext context) : base(context)
        {
        }

        public async Task<MultipleChoicesQuiz> GetById(string id)
        {
            var quiz = (MultipleChoicesQuiz)(await _dbSet.Find(r => r.Id == id).FirstOrDefaultAsync());
            if(quiz is null)
            {
                return null;
            }

            var options = quiz.Options;

            Random rand = new Random();
            options = options.OrderBy(x => rand.Next()).ToList();

            quiz.Options = options;
            return quiz;
        }

        public async Task Update(MultipleChoicesQuiz quiz)
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
    }
}
