using AutoMapper;
using QuizesManagement.BuisnessLogicLayer.Models;
using QuizesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public class ManualQuizCreationStrategy : IQuizCreationStrategy
    {
        private readonly IMapper _mapper;

        public ManualQuizCreationStrategy(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<MultipleChoicesQuiz> CreateMultipleChoicesQuiz(object parameters)
        {
            if (parameters is not AddMultipleChoicesQuizRequest request)
                throw new ArgumentException("Invalid parameter type for Manual MCQ creation.", nameof(parameters));

            var quiz = _mapper.Map<MultipleChoicesQuiz>(request);
            return Task.FromResult(quiz);
        }

        public Task<TrueOrFalseQuiz> CreateTrueOrFalseQuiz(object parameters)
        {
            if (parameters is not AddTrueOrFalseQuizRequest request)
                throw new ArgumentException("Invalid parameter type for Manual TFQ creation.", nameof(parameters));

            var quiz = _mapper.Map<TrueOrFalseQuiz>(request);
            return Task.FromResult(quiz);
        }
    }
}
