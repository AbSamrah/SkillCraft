using QuizesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Models
{
    public class TrueOrFalseQuizDto : TrueOrFalseQuizRequest
    {
        public string Id { get; set; }
    }
}
