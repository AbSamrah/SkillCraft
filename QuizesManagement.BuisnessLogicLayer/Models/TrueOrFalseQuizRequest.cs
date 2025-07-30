using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Models
{
    public class TrueOrFalseQuizRequest : QuizRequest
    {
        public bool? Answer { get; set; }

        public string Type { get; set; } = "TrueOrFalseQuiz";
    }
}
