using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class MilestoneRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
