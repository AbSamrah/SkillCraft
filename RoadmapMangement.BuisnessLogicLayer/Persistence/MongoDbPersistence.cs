using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using RoadmapMangement.BuisnessLogicLayer.Persistence;
using System;

namespace RoadmapManagement.BusinessLogicLayer.Persistence
{
    public static class MongoDbPersistence
    {
        private static bool _initialized = false;
        private static readonly object _lock = new object();

        public static void Configure()
        {
            lock (_lock)
            {
                if (_initialized) return;

                try
                {
                    // Configure class maps first
                    RoadmapMap.Configure();

                    // For MongoDB .NET Driver v2.11+ - New way to handle GUID serialization
                    var pack = new ConventionPack
                    {
                        new IgnoreExtraElementsConvention(true),
                        new IgnoreIfDefaultConvention(true),
                        new CamelCaseElementNameConvention(),
                        new EnumRepresentationConvention(BsonType.String)
                    };

                    ConventionRegistry.Register(
                        "SolutionConventions",
                        pack,
                        t => true);

                    // Register GUID serializer (new way)
                    BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

                    _initialized = true;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("MongoDB configuration failed", ex);
                }
            }
        }
    }
}