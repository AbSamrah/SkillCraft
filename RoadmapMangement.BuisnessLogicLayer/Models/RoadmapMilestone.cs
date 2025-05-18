using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class RoadmapMilestone
    {
        public string RoadmapId { get; set; }
        public List<string> MilestonesIds { get; set; }
    }
}
