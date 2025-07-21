using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Models
{
    public class EditMultipleChoicesQuizRequest: MultipleChoicesQuizRequest
    {
        public string Id { get; set; }
    }
}
