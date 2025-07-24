using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.DataAccessLayer.Models
{
    public class Step : Entity
    {
        [BsonElement("isCompleted")]
        public bool IsCompleted { get; set; }

        [BsonElement("durationInMinutes")]
        public int DurationInMinutes { get; set; }
    }
}
