using AutoMapper;
using QuizesManagement.BuisnessLogicLayer.Models;
using QuizesManagement.DataAccessLayer.Interfaces;
using QuizesManagement.DataAccessLayer.Models;
using QuizesManagement.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public class TrueOrFlaseQuizService : QuizService, ITrueOrFalseQuizService
    {
        private readonly ITrueOrFalseQuizRepository _trueOrFalseQuizRepository;

        public TrueOrFlaseQuizService(ITrueOrFalseQuizRepository trueOrFalseQuizRepository, IMapper mapper, IUnitOfWork uow) : base(trueOrFalseQuizRepository, mapper, uow)
        {
            _trueOrFalseQuizRepository = trueOrFalseQuizRepository;
        }

        public async Task<TrueOrFalseQuizDto> Add(IQuizCreationStrategy strategy, object parameters)
        {
            var quiz = await strategy.CreateTrueOrFalseQuiz(parameters);
            quiz.Type = "TrueOrFalse";
            _quizRepository.Add(quiz);
            await _uow.Commit();
            return _mapper.Map<TrueOrFalseQuizDto>(quiz);
        }

        public async Task<bool> CheckAnswer(string quizId, bool answer)
        {
            var quiz = await _trueOrFalseQuizRepository.GetById(quizId);
            if (quiz == null)
            {
                throw new Exception("Quiz not found.");
            }

            return answer == quiz.Answer;
        }

        public async Task<TrueOrFalseQuizDto> GetById(string id)
        {
            var quiz = await _trueOrFalseQuizRepository.GetById(id);

            if (quiz is null)
            {
                throw new Exception("Quiz not found.");
            }

            TrueOrFalseQuizDto quizDto = _mapper.Map<TrueOrFalseQuizDto>(quiz);
            return quizDto;
        }

        public async Task<TrueOrFalseQuizDto> Update(EditTrueOrFalseQuizRequest quizRequest)
        {
            var existingQuiz = await _trueOrFalseQuizRepository.GetById(quizRequest.Id);

            if (existingQuiz is null)
            {
                throw new Exception("Quiz not found.");
            }

            existingQuiz.Question = quizRequest.Question;
            existingQuiz.Answer = (quizRequest.Answer == true);
            existingQuiz.Tags = new List<string>();
            foreach (var tag in quizRequest.Tags)
            {
                existingQuiz.Tags.Add(tag);
            }


            await _trueOrFalseQuizRepository.Update(existingQuiz);
            await _uow.Commit();

            TrueOrFalseQuizDto multipleChoicesQuizDto = _mapper.Map<TrueOrFalseQuizDto>(existingQuiz);
            return multipleChoicesQuizDto;
        }
    }
}
