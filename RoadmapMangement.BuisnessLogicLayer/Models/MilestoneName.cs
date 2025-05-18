using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class MilestoneName
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<StepName> Steps { get; set; }

    }
}
