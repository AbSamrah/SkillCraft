using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.DataAccessLayer.Models
{
    public class MultipleChoicesQuiz: Quiz
    {
        [BsonElement("options")]
        public List<string> OptionsIds { get; set; } = new List<string>();
        [BsonIgnore]
        public List<Option> Options { get; set; } = new List<Option>();
        [BsonElement("Answer")]
        public string AnswerId { get; set; }
    }
}
