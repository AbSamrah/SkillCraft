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
            CreateMap<Quiz, QuizDto>();

            CreateMap<QuizRequest, Quiz>();

            CreateMap<MultipleChoicesQuiz, MultipleChoicesQuizDto>();

            CreateMap<MultipleChoicesQuizRequest, MultipleChoicesQuiz>();
        }
    }
}
