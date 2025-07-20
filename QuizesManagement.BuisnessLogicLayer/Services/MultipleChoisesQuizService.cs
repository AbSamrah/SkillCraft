using AutoMapper;
using QuizesManagement.BuisnessLogicLayer.Models;
using QuizesManagement.DataAccessLayer.Interfaces;
using QuizesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public class MultipleChoisesQuizService: IQuizService
    {
        private readonly IMultipleChoicesQuizRepository _quizRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MultipleChoisesQuizService(IMultipleChoicesQuizRepository quizRepository, IUnitOfWork uow, IMapper mapper)
        {
            _quizRepository = quizRepository;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<QuizDto> Add(QuizRequest quizRequest)
        {
            MultipleChoicesQuizRequest multipleChoicesQuizRequest = (MultipleChoicesQuizRequest)quizRequest;
            MultipleChoicesQuiz multipleChoicesQuiz = _mapper.Map<MultipleChoicesQuiz>(multipleChoicesQuizRequest);

            foreach (string option in multipleChoicesQuizRequest.Options)
            {
                multipleChoicesQuiz.OptionsIds.Add(option);
            }

            _quizRepository.Add(multipleChoicesQuiz);
            await _uow.Commit();

            MultipleChoicesQuizDto multipleChoicesQuizDto = _mapper.Map<MultipleChoicesQuizDto>(multipleChoicesQuiz);
            return multipleChoicesQuizDto;
        }

        public Task<bool> CheckAnswer(QuizDto quiz, IAnswer answer)
        {
            throw new NotImplementedException();
        }

        public Task<QuizDto> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<QuizDto> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<QuizDto>> GetAllQuizes()
        {
            throw new NotImplementedException();
        }

        public Task<QuizDto> Update(QuizRequest quizRequest)
        {
            throw new NotImplementedException();
        }
    }
}
