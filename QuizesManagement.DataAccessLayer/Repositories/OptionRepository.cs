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
    public class OptionRepository
    {
        protected readonly IQuizDbContext _context;
        protected IMongoCollection<Option> _dbSet;

        public OptionRepository(IQuizDbContext context)
        {
            _context = context;

            _dbSet = _context.GetCollection<Option>(typeof(Option).Name); ;
        }
    }
}
