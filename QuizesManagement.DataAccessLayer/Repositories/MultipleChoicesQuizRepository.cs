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

        public virtual async Task<MultipleChoicesQuiz> GetById(string id)
        {
            var quiz = (MultipleChoicesQuiz)(await _dbSet.Find(r => r.Id == id).FirstOrDefaultAsync());
            if (quiz == null || quiz.OptionsIds?.Any() != true)
                return quiz;

            var options = await _context.GetCollection<Option>("Option")
                .Find(m => quiz.OptionsIds.Contains(m.Id))
                .ToListAsync();

            Random rand = new Random();
            options = options.OrderBy(x => rand.Next()).ToList();

            quiz.Options = options;
            return quiz;
        }
    }
}
