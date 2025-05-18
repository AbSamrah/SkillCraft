using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class MilestoneDto: MilestoneRequest
    {
        public string Id { get; set; }
        public List<StepDto> Steps { get; set; }
        // other properties
    }
}
