using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.DataAccessLayer.Models
{
    public class Roadmap: Entity
    {
        public List<string> Tags { get; set; }
        public List<Milestone> Milestones { get; set; }
        public decimal Salary { get; set; }
    }
}
