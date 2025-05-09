using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using RoadmapMangement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Persistence
{
    public class RoadmapMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Step>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId)) 
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
                map.MapMember(x => x.Description).SetIsRequired(true);
            });
        }
    }
}
