using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class AddMilestoneRequest: MilestoneRequest
    {
        public List<string> StepsIds { get; set; }
    }
}
