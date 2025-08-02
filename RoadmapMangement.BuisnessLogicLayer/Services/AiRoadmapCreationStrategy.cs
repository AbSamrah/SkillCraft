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
                throw new ArgumentException("Invalid parameter type for AiRoadmapCreationStrategy.", nameof(parameters));
            }

            var prompt = ConstructPrompt(aiParams.Prompt, aiParams.CompletedSteps);
            var generatedJson = await _aiGenerator.GenerateJsonAsync(prompt);
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
                Salary = generatedRoadmap.AverageSalary
            };

            foreach (var milestoneData in generatedRoadmap.Milestones)
            {
                var milestone = new Milestone { Name = milestoneData.Title, Description = milestoneData.Description };

                foreach (var stepData in milestoneData.Steps)
                {
                    // ** This is the new, smarter logic **
                    if (stepData.IsCompleted && !string.IsNullOrEmpty(stepData.OriginalId))
                    {
                        // If the step is marked as completed, just link the existing step ID.
                        milestone.StepsIds.Add(stepData.OriginalId);
                    }
                    else
                    {
                        // Otherwise, create a new step as before.
                        var newStep = new Step { Name = stepData.Title, Description = stepData.Description, DurationInMinutes = stepData.DurationInMinutes };
                        _stepRepository.Add(newStep);
                        milestone.StepsIds.Add(newStep.Id);
                    }
                }
                _milestoneRepository.Add(milestone);
                roadmap.MilestonesIds.Add(milestone.Id);
            }

            // Commit only the NEW steps and milestones
            await _uow.Commit();
            return roadmap;
        }

        private string ConstructPrompt(string skill, List<StepDto> completedSteps)
        {
            // Convert the list of completed steps into a simple string format for the prompt.
            var completedStepsText = completedSteps.Any()
                ? string.Join(", ", completedSteps.Select(s => $"{{ \"id\": \"{s.Id}\", \"name\": \"{s.Name}\" }}"))
                : "None";

            return $@"
                You are an expert technical curriculum designer. Your task is to generate a personalized learning roadmap for a '{skill}' developer. Your primary goal is to build upon the user's existing knowledge.

                The response MUST be a single, well-formed JSON object and nothing else.

                **Existing Steps Already Completed by the User:**
                [
                  {completedStepsText}
                ]

                **JSON Structure Required:**
                {{
                  ""title"": ""A Creative Title for a Personalized {skill} Roadmap"",
                  ""description"": ""A detailed description of the role and this personalized learning path."",
                  ""averageSalary"": 115000,
                  ""salaryCurrency"": ""USD"",
                  ""tags"": [""tag1"", ""tag2""],
                  ""milestones"": [
                    {{
                      ""title"": ""Milestone Name"",
                      ""description"": ""Milestone objectives."",
                      ""steps"": [
                        {{
                          ""title"": ""Step Name"",
                          ""description"": ""Detailed description of the concept."",
                          ""durationInMinutes"": 120,
                          ""isCompleted"": false,
                          ""originalId"": null
                        }}
                      ]
                    }}
                  ]
                }}

                **CRITICAL Instructions:**
                1.  **Prioritize Existing Steps**: Your most important task is to reuse the user's completed steps. Before generating a new step, check if a similar concept already exists in the ""User's Completed Steps"" list. If it does, you MUST use the existing step.
                2.  **Incorporate Relevant Steps**: If a user's completed step is a logical part of the new '{skill}' roadmap, you MUST include it in your response. When you do, you MUST set ""isCompleted"" to true and you MUST copy its original ""id"" into the ""originalId"" field. Do not change the original title or description.
                3.  **Generate Only Necessary New Steps**: Only generate a new step if the required knowledge does not exist in the user's completed steps list. For all new steps, ""isCompleted"" MUST be false and ""originalId"" MUST be null.
                4.  **Logical Flow**: Arrange all steps (both existing and new) into logical, thematic milestones that flow from beginner to advanced. A milestone can and should contain a mix of completed and new steps if it makes sense.
                5.  **Ignore Irrelevant Steps**: If a user's completed step is completely unrelated to the '{skill}' roadmap (e.g., 'Learn to Bake Bread' for a 'Data Scientist' roadmap), you must ignore it and not include it in the output.
                6.  **Follow All Other Rules**: Adhere to all previous rules regarding a creative title, detailed description, salary, a comprehensive list of tags, and granular, specific steps.
            ";
        }
    }
}
