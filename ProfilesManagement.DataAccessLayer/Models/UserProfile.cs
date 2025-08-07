using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilesManagement.DataAccessLayer.Models
{
    public class UserProfile
    {
        public UserProfile(string userId)
        {
            Id = userId;
            Roadmaps = new List<RoadmapStatus>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("roadmaps")]
        public List<RoadmapStatus> Roadmaps { get; set; } = new List<RoadmapStatus>();

        [BsonElement("steps")]
        public List<string> FinishedSteps { get; set; } = new List<string>();

        [BsonElement("quizzes")]
        public List<string> Quizzes { get; set; } = new List<string>();

        [BsonElement("energy")]
        public int Energy { get; set; } = 100;

        [BsonElement("lastEnergyRefill")]
        public DateTime LastEnergyRefill { get; set; } = DateTime.UtcNow;
    }
}
