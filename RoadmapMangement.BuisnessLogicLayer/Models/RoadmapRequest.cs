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
    public class RoadmapRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Salary must be greater than zero.")]
        public decimal Salary { get; set; }
    }
}
