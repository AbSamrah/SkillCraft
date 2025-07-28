using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Filters
{
    public class RoadmapFilter
    {
        public string? Name { get; set; }
        public List<string>? Tags { get; set; }
        public decimal MaxSalary { get; set; } = decimal.MaxValue;
        public decimal MinSalary { get; set; } = 0;
        public bool IsActive { get; set; } = false;
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
