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
    public class AiRoadmapCreationStrategy : IRoadmapCreationStrategy
    {
        private readonly IAiGenerator _aiGenerator;
        private readonly IRepository<Milestone> _milestoneRepository;
        private readonly IRepository<Step> _stepRepository;
        private readonly IUnitOfWork _uow;

        public AiRoadmapCreationStrategy(
            IAiGenerator aiGenerator,
            IRepository<Milestone> milestoneRepository,
            IRepository<Step> stepRepository,
            IUnitOfWork uow)
        {
            _aiGenerator = aiGenerator;
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

            // The strategy's only interaction with the AI is through this clean interface.
            var generatedJson = await _aiGenerator.GenerateJsonAsync(prompt);

            var generatedRoadmap = JsonSerializer.Deserialize<AiGeneratedRoadmap>(generatedJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (generatedRoadmap is null)
            {
                throw new InvalidOperationException("Failed to deserialize the AI's JSON response.");
            }

            // This business logic (saving to DB) is the core responsibility of the strategy now.
            var roadmap = new Roadmap
            {
                Name = generatedRoadmap.Title,
                Description = generatedRoadmap.Description,
                IsActive = true,
                Tags = generatedRoadmap.Tags,
                Salary = generatedRoadmap.AverageSalary
            };

            foreach (var milestoneData in generatedRoadmap.Milestones)
            {
                var milestone = new Milestone { Name = milestoneData.Title, Description = milestoneData.Description };
                foreach (var stepData in milestoneData.Steps)
                {
                    var step = new Step { Name = stepData.Title, Description = stepData.Description, DurationInMinutes = stepData.DurationInMinutes };
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
            // The prompt engineering logic remains part of the strategy.
            return $@"
                You are an expert technical curriculum designer and career advisor. Your task is to generate a comprehensive, high-quality learning roadmap for a '{skill}' developer.
                The response MUST be a single, well-formed JSON object and nothing else.
                The JSON object must adhere to this exact structure:
                {{
                  ""title"": ""A Creative and Descriptive Title for a {skill} Roadmap"",
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
                1.  **Title**: Generate a creative and inspiring title for the roadmap.
                2.  **Description**: Write a detailed, engaging paragraph about the role and the roadmap.
                3.  **Salary**: Provide a realistic, estimated average annual salary for a mid-level {skill} developer in the United States, with ""USD"" as the currency.
                4.  **Tags**: Generate a comprehensive list of 10 to 15 relevant, lowercase technical tags.
                5.  **Milestones**: Structure the roadmap into logical, thematic milestones (e.g., ""Version Control Foundations"", ""API Development"").
                6.  **Steps**: Steps must be highly specific and granular.
                7.  **Durations**: Estimate learning time for each step in minutes, assuming a standard learning day is 8 hours (480 minutes).
            ";
        }
    }
}
