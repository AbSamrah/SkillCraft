using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuizesManagement.BuisnessLogicLayer.Models
{
    public abstract class QuizDto
    {
        public string Id { get; set; }
        
        public string Question { get; set; }
        
        public string Tag { get; set; }
        
        public bool IsCompleted { get; set; }
    }
}
