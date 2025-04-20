using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLogicLayer.Filters
{
    public class UserFilter
    {
        public string? Email { get; set; } = null;
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
