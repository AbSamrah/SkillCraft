﻿using QuizesManagement.BuisnessLogicLayer.Filters;
using QuizesManagement.BuisnessLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public interface IQuizService
    {
        public Task<List<QuizDto>> GetAll(QuizFilter filter, List<string> finishedQuizzes);

        public Task<QuizDto> Delete(string id);

        //public Task<QuizDto> GetById(string id);
    }
}
