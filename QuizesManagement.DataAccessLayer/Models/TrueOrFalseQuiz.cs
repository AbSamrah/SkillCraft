using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.DataAccessLayer.Models
{
    public class TrueOrFalseQuiz: Quiz
    {
        [BsonElement("answer")]
        public bool Answer { get; set; }
    }
}
