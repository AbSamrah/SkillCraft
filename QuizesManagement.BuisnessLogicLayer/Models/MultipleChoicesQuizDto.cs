using QuizesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Models
{
    public class MultipleChoicesQuizDto: QuizDto
    {
        public List<Option> Options { get; set; }
    }
}
