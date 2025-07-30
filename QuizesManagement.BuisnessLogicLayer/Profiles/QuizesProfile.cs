using AutoMapper;
using QuizesManagement.BuisnessLogicLayer.Models;
using QuizesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Profiles
{
    public class QuizesProfile: Profile
    {
        public QuizesProfile()
        {
            // General
            CreateMap<Quiz, QuizDto>();
            CreateMap<QuizRequest, Quiz>();

            // Multiple Choice
            CreateMap<MultipleChoicesQuiz, MultipleChoicesQuizDto>();
            CreateMap<AddMultipleChoicesQuizRequest, MultipleChoicesQuiz>();
            CreateMap<EditMultipleChoicesQuizRequest, MultipleChoicesQuiz>();
            CreateMap<AiGeneratedMcq, MultipleChoicesQuiz>();

            // True or False
            CreateMap<TrueOrFalseQuiz, TrueOrFalseQuizDto>();
            CreateMap<AddTrueOrFalseQuizRequest, TrueOrFalseQuiz>();
            CreateMap<EditTrueOrFalseQuizRequest, TrueOrFalseQuiz>();
            CreateMap<AiGeneratedTfq, TrueOrFalseQuiz>();
        }
    }
}
