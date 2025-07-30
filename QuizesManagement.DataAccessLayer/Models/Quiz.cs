using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuizesManagement.DataAccessLayer.Models
{
    [BsonKnownTypes(typeof(MultipleChoicesQuiz))]
    public class Quiz
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement("question")]
        public string Question {  get; set; }
        [BsonElement("tags")]
        public List<string> Tags {  get; set; }
        [BsonElement("_t")]
        public string Type { get; set; }
    }
}
