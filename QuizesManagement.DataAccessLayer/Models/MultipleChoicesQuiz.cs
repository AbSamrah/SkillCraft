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
        public List<string> Options { get; set; } = new List<string>();
        [BsonElement("answer")]
        public string Answer { get; set; }
    }
}
