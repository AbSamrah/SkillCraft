using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using QuizesManagement.BuisnessLogicLayer.Filters;
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
    public class QuizService : IQuizService
    {
        protected readonly IQuizRepository _quizRepository;
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _uow;

        public QuizService(IQuizRepository quizRepository, IMapper mapper, IUnitOfWork uow) 
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
            _uow = uow;
        }

        

        public async Task<QuizDto> Delete(string id)
        {
            var quiz = await _quizRepository.GetById(id);

            if(quiz is null)
            {
                throw new Exception("Quiz not found.");
            }

            QuizDto quizDto = _mapper.Map<QuizDto>(quiz);
            _quizRepository.Remove(id);
            await _uow.Commit();
            return quizDto;
        }

        public async Task<List<QuizDto>> GetAll(QuizFilter filter)
        {
            var quizzesFromDb = await _quizRepository.GetAll(filter.Tags, filter.PageNumber, filter.PageSize);
            var quizDtos = new List<QuizDto>();

            foreach (var quiz in quizzesFromDb)
            {
                if (quiz is MultipleChoicesQuiz mcq)
                {
                    quizDtos.Add(_mapper.Map<MultipleChoicesQuizDto>(mcq));
                }
                else if (quiz is TrueOrFalseQuiz tfq)
                {
                    quizDtos.Add(_mapper.Map<TrueOrFalseQuizDto>(tfq));
                }
                else
                {
                    quizDtos.Add(_mapper.Map<QuizDto>(quiz));
                }
            }

            return quizDtos;
        }
    }
    
}
