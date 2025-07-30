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
    public class MultipleChoisesQuizService : QuizService, IMultipleChoisesQuizService
    {
        private readonly IMultipleChoicesQuizRepository _multipleChoicesQuizRepository;

        public MultipleChoisesQuizService(IMultipleChoicesQuizRepository multipleChoicesQuizRepository, IMapper mapper, IUnitOfWork uow) : base(multipleChoicesQuizRepository , mapper, uow)
        {
            _multipleChoicesQuizRepository = multipleChoicesQuizRepository;
        }

        public async Task<MultipleChoicesQuizDto> Add(IQuizCreationStrategy strategy, object parameters)
        {
            var quiz = await strategy.CreateMultipleChoicesQuiz(parameters);
            quiz.Type = "MultipleChoices";
            _quizRepository.Add(quiz);
            await _uow.Commit();
            return _mapper.Map<MultipleChoicesQuizDto>(quiz);
        }

        public async Task<bool> CheckAnswer(string quizId, string answer)
        {
            var quiz = await _multipleChoicesQuizRepository.GetById(quizId);
            if (quiz == null)
            {
                throw new Exception("Quiz not found.");
            }

            return quiz.Answer == answer;

        }

        public async Task<MultipleChoicesQuizDto> GetById(string id)
        {
            var quiz = await _multipleChoicesQuizRepository.GetById(id);

            if (quiz is null)
            {
                throw new Exception("Quiz not found.");
            }

            MultipleChoicesQuizDto quizDto = _mapper.Map<MultipleChoicesQuizDto>(quiz);
            return quizDto;
        }

        public async Task<MultipleChoicesQuizDto> Update(EditMultipleChoicesQuizRequest quizRequest)
        {
            var existingQuiz = await _multipleChoicesQuizRepository.GetById(quizRequest.Id);

            if (existingQuiz is null)
            {
                throw new Exception("Quiz not found.");
            }

            existingQuiz.Question = quizRequest.Question;
            existingQuiz.Answer = quizRequest.Answer;
            existingQuiz.Tags = new List<string>();
            foreach (var tag in quizRequest.Tags)
            {
                existingQuiz.Tags.Add(tag);
            }
            existingQuiz.Options = new List<string>();

            foreach (string option in quizRequest.Options)
            {
                existingQuiz.Options.Add(option);
            }

            await _multipleChoicesQuizRepository.Update(existingQuiz);
            await _uow.Commit();

            MultipleChoicesQuizDto multipleChoicesQuizDto = _mapper.Map<MultipleChoicesQuizDto>(existingQuiz);
            return multipleChoicesQuizDto;
        }
    }
}
