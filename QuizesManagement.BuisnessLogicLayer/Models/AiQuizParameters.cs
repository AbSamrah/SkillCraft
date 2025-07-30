using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Models
{
    public class AiQuizParameters
    {
        [Required]
        public string Topic { get; set; }

        [Required]
        public string Difficulty { get; set; }
    }
}
