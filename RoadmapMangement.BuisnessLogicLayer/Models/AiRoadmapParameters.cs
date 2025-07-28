using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class AiRoadmapParameters
    {
        [Required]
        [MinLength(5)]
        public string Prompt { get; set; }
    }
}
