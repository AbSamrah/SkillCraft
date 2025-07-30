using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Models
{
    public class AiGeneratedTfq
    {
        [JsonPropertyName("question")]
        public string Question { get; set; }

        [JsonPropertyName("answer")]
        public bool Answer { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }
    }
}
