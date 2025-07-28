using Microsoft.Extensions.Configuration;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.BuisnessLogicLayer.Models.Gemini;
using RoadmapMangement.DataAccessLayer.Interfaces;
using RoadmapMangement.DataAccessLayer.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace RoadmapMangement.BuisnessLogicLayer.Services
{
    /// <summary>
    /// A strategy for creating a complete, structured roadmap using the Google Gemini AI.
    /// </summary>
    public class AiRoadmapCreationStrategy : IRoadmapCreationStrategy
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Milestone> _milestoneRepository;
        private readonly IRepository<Step> _stepRepository;
        private readonly IUnitOfWork _uow;

        public AiRoadmapCreationStrategy(
            HttpClient httpClient,
            IConfiguration configuration,
            IRepository<Milestone> milestoneRepository,
            IRepository<Step> stepRepository,
            IUnitOfWork uow)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _milestoneRepository = milestoneRepository;
            _stepRepository = stepRepository;
            _uow = uow;
        }

        public async Task<Roadmap> CreateRoadmap(object parameters)
        {
            if (parameters is not AiRoadmapParameters aiParams)
            {
                throw new ArgumentException("Invalid parameter type for AiRoadmapCreationStrategy. Expected AiRoadmapParameters.", nameof(parameters));
            }

            var prompt = ConstructPrompt(aiParams.Prompt);
            var generatedJson = await CallGeminiApi(prompt);
            var generatedRoadmap = JsonSerializer.Deserialize<AiGeneratedRoadmap>(generatedJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (generatedRoadmap is null)
            {
                throw new InvalidOperationException("Failed to deserialize the AI's JSON response.");
            }

            var roadmap = new Roadmap
            {
                Name = generatedRoadmap.Title,
                Description = generatedRoadmap.Description,
                IsActive = true,
                Tags = generatedRoadmap.Tags,
                Salary = generatedRoadmap.AverageSalary // Map the new salary property
            };

            foreach (var milestoneData in generatedRoadmap.Milestones)
            {
                var milestone = new Milestone
                {
                    Name = milestoneData.Title,
                    Description = milestoneData.Description
                };

                foreach (var stepData in milestoneData.Steps)
                {
                    var step = new Step
                    {
                        Name = stepData.Title,
                        Description = stepData.Description,
                        DurationInMinutes = stepData.DurationInMinutes
                    };
                    _stepRepository.Add(step);
                    milestone.StepsIds.Add(step.Id);
                }
                _milestoneRepository.Add(milestone);
                roadmap.MilestonesIds.Add(milestone.Id);
            }

            await _uow.Commit();
            return roadmap;
        }

        private string ConstructPrompt(string skill)
        {
            return $@"
                You are an expert technical curriculum designer and career advisor. Your task is to generate a comprehensive, high-quality learning roadmap for a '{skill}' developer.

                The response MUST be a single, well-formed JSON object and nothing else. Do not include any text or markdown formatting like ```json before or after the JSON object.

                The JSON object must adhere to this exact structure:
                {{
                  ""title"": ""The Complete {skill} Developer Roadmap"",
                  ""description"": ""A detailed description of the {skill} role, its industry importance, and an overview of this learning path."",
                  ""averageSalary"": 115000,
                  ""salaryCurrency"": ""USD"",
                  ""tags"": [""tag1"", ""tag2"", ""tag3"", ""...""],
                  ""milestones"": [
                    {{
                      ""title"": ""Milestone Name"",
                      ""description"": ""A summary of this milestone's learning objectives."",
                      ""steps"": [
                        {{
                          ""title"": ""Specific Step Name"",
                          ""description"": ""A detailed, actionable description of the concept to learn or task to perform."",
                          ""durationInMinutes"": 240
                        }}
                      ]
                    }}
                  ]
                }}

                **Content Generation Rules:**
                1.  **Description**: Write a detailed, engaging paragraph. Explain what a {skill} developer does, why the role is valuable, and what the user will learn by following this roadmap from start to finish.
                2.  **Salary**: Provide a realistic, estimated average annual salary for a mid-level {skill} developer in the United States. The currency must be ""USD"".
                3.  **Tags**: Generate a comprehensive list of 10 to 15 relevant, lowercase technical tags. These should be specific technologies, concepts, or tools related to the {skill} role.
                4.  **Milestones**: Structure the roadmap into logical, thematic milestones. Do not use generic phases like ""Beginner"" or ""Advanced"". Instead, group topics by concept (e.g., ""Version Control Foundations"", ""Core Language Proficiency"", ""API Development"", ""Testing and QA""). Each milestone's steps must be closely related to its central theme.
                5.  **Steps**: This is crucial. Steps must be highly specific and granular. Break down large topics into smaller, manageable learning chunks. For example, instead of a step called ""Learn CSS"", create multiple steps like ""CSS Box Model"", ""Flexbox Layouts"", ""CSS Grid"", and ""Responsive Design with Media Queries"". The number of steps per milestone should be determined by the topic's complexity, not a fixed number.
                6.  **Durations**: Estimate the learning time for each step in minutes. Assume a standard learning day is 8 hours (480 minutes). Be realistic; a complex topic should have a longer duration.
            ";
        }

        private async Task<string> CallGeminiApi(string prompt)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("Gemini API key is not configured.");
            }

            var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            var requestPayload = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };

            var response = await _httpClient.PostAsJsonAsync(apiUrl, requestPayload);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to generate roadmap using AI. Status: {response.StatusCode}, Details: {errorContent}");
            }

            var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
            var generatedText = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

            if (string.IsNullOrWhiteSpace(generatedText))
            {
                throw new InvalidOperationException("AI returned an empty or invalid response.");
            }

            return generatedText.Trim().Replace("```json", "").Replace("```", "");
        }
    }
}
