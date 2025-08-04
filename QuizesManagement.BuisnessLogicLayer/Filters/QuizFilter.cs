using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Filters
{
    public class QuizFilter
    {
        public string? Name { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
