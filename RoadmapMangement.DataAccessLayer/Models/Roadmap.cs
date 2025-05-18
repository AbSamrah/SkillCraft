using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;
using System.Text.Json.Serialization;

namespace RoadmapMangement.DataAccessLayer.Models
{
    public class Roadmap : Entity
    {
        [BsonElement("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        // Reference milestones by ID (better for large collections)
        [BsonElement("milestoneIds")]
        public List<string> MilestoneIds { get; set; } = new List<string>();

        // Ignored property to be populated when needed
        [BsonIgnore]
        public List<Milestone> Milestones { get; set; } = new List<Milestone>();

        [BsonElement("salary")]
        public decimal Salary { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
    }
}
