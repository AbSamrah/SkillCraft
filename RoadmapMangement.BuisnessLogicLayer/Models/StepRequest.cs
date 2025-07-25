using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class StepRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
