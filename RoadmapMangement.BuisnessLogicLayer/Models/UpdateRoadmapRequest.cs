﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class UpdateRoadmapRequest: RoadmapRequest
    {
        public List<string> MilestonesIds { get; set; } = new List<string>();
        public bool IsActive { get; internal set; }
    }
}
