﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RoadmapMangement.BuisnessLogicLayer.Models
{
    public class AiGeneratedRoadmap
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        [JsonPropertyName("averageSalary")]
        public int AverageSalary { get; set; }

        [JsonPropertyName("salaryCurrency")]
        public string SalaryCurrency { get; set; }

        [JsonPropertyName("milestones")]
        public List<AiGeneratedMilestone> Milestones { get; set; }
    }

    public class AiGeneratedMilestone
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("steps")]
        public List<AiGeneratedStep> Steps { get; set; }
    }

    public class AiGeneratedStep
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("durationInMinutes")]
        public int DurationInMinutes { get; set; }

        [JsonPropertyName("isCompleted")]
        public bool IsCompleted { get; set; }

        [JsonPropertyName("originalId")]
        public string OriginalId { get; set; }
    }
}
