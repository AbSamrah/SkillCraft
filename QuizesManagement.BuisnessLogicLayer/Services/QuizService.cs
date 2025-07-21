using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
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

        public async Task<List<QuizDto>> GetAll()
        {
            var quizes = await _quizRepository.GetAll();

            return _mapper.Map<List<QuizDto>>(quizes);
        }

        /*public async Task<QuizDto> GetById(string id)
        {
            var quiz = await _quizRepository.GetById(id);
            if (quiz is null)
            {
                throw new Exception("Quiz not found.");
            }

            QuizDto quizDto = _mapper.Map<QuizDto>(quiz);

            return quizDto;
        }*/
    }
    
}
