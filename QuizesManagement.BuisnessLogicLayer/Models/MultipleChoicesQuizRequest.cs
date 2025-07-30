using QuizesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Models
{
    public class MultipleChoicesQuizRequest: QuizRequest
    {
        public List<string> Options { get; set; }
        public string? Answer { get; set; }
        public string Type { get; set; } = "MultipleChoices";
    }
}
