using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class MilestoneStep
    {
        public string MilestoneId { get; set; }
        public List<string> StepIds { get; set; }
    }
}
