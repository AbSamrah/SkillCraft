using AutoMapper;
using QuizesManagement.BuisnessLogicLayer.Models;
using QuizesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public class AiQuizCreationStrategy : IQuizCreationStrategy
    {
        private readonly IAiGenerator _aiGenerator;
        private readonly IMapper _mapper;

        public AiQuizCreationStrategy(IAiGenerator aiGenerator, IMapper mapper)
        {
            _aiGenerator = aiGenerator;
            _mapper = mapper;
        }

        public async Task<MultipleChoicesQuiz> CreateMultipleChoicesQuiz(object parameters)
        {
            if (parameters is not AiQuizParameters aiParams)
                throw new ArgumentException("Invalid parameter type for AI MCQ creation.", nameof(parameters));

            var prompt = ConstructMcqPrompt(aiParams);
            var generatedJson = await _aiGenerator.GenerateJsonAsync(prompt);
            var aiGeneratedMcq = JsonSerializer.Deserialize<AiGeneratedMcq>(generatedJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (aiGeneratedMcq is null)
                throw new InvalidOperationException("Failed to deserialize AI response for MCQ.");

            return _mapper.Map<MultipleChoicesQuiz>(aiGeneratedMcq);
        }

        public async Task<TrueOrFalseQuiz> CreateTrueOrFalseQuiz(object parameters)
        {
            if (parameters is not AiQuizParameters aiParams)
                throw new ArgumentException("Invalid parameter type for AI TFQ creation.", nameof(parameters));

            var prompt = ConstructTfqPrompt(aiParams);
            var generatedJson = await _aiGenerator.GenerateJsonAsync(prompt);
            var aiGeneratedTfq = JsonSerializer.Deserialize<AiGeneratedTfq>(generatedJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (aiGeneratedTfq is null)
                throw new InvalidOperationException("Failed to deserialize AI response for TFQ.");

            return _mapper.Map<TrueOrFalseQuiz>(aiGeneratedTfq);
        }

        private string ConstructMcqPrompt(AiQuizParameters aiParams)
        {
            return $@"
                You are a quiz generation expert. Your task is to create a single, high-quality multiple-choice question.
                The response MUST be a single, well-formed JSON object and nothing else. Do not include any text or markdown formatting like ```json before or after the JSON object.

                **JSON Structure Example:**
                {{
                  ""question"": ""What is the purpose of the 'async' keyword in C#?"",
                  ""options"": [""To declare a method as asynchronous"", ""To block the current thread"", ""To handle exceptions automatically"", ""To define a constructor""],
                  ""answer"": ""To declare a method as asynchronous"",
                  ""tags"": [""csharp"", ""asynchronous"", ""programming""]
                }}

                **Instructions for the new quiz:**
                1.  **Topic**: The question must be about '{aiParams.Topic}'.
                2.  **Difficulty**: The difficulty level must be '{aiParams.Difficulty}'.
                3.  **Options**: Provide exactly 4 plausible options. Only one can be correct.
                4.  **Answer**: The 'answer' field must exactly match one of the strings in the 'options' array.
                5.  **Tags**: The 'tags' field MUST be a JSON array containing 2-3 relevant, lowercase, single-word strings.
            ";
        }

        private string ConstructTfqPrompt(AiQuizParameters aiParams)
        {
            return $@"
                You are a quiz generation expert. Your task is to create a single, high-quality true or false question.
                The response MUST be a single, well-formed JSON object and nothing else. Do not include any text or markdown formatting like ```json before or after the JSON object.

                **JSON Structure Example:**
                {{
                  ""question"": ""In C#, a 'struct' is a reference type."",
                  ""answer"": false,
                  ""tags"": [""csharp"", ""dotnet"", ""types""]
                }}

                **Instructions for the new quiz:**
                1.  **Topic**: The question must be about '{aiParams.Topic}'.
                2.  **Difficulty**: The difficulty level must be '{aiParams.Difficulty}'.
                3.  **Answer**: The 'answer' field must be a boolean value (true or false).
                4.  **Tags**: The 'tags' field MUST be a JSON array containing 2-3 relevant, lowercase, single-word strings.
            ";
        }
    }
}
