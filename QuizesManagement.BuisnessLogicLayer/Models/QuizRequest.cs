using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuizesManagement.BuisnessLogicLayer.Models
{
    public class QuizRequest
    {
        
        public string Question { get; set; }
        
        public string Tag { get; set; }
    }
}
