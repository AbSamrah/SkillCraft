﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuizesManagement.BuisnessLogicLayer.Models
{
    public class QuizDto: QuizRequest
    {
        public string Id { get; set; }

        public string Type { get; set; }
    }
}
