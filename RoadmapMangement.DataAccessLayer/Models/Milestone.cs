using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RoadmapMangement.DataAccessLayer.Models
{
    public class Milestone : Entity
    {
        [BsonElement("steps")]
        public List<string> StepsIds { get; set; } = new List<string>();
        [BsonIgnore]
        public List<Step> Steps { get; set; } = new List<Step>();

        [BsonIgnore]
        public int DurationInMinutes { get; set; }
    }
}
