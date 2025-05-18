using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class UpdateMilestoneRequest: MilestoneRequest
    {
        public List<string> StepsIds { get; set; }
    }
}
