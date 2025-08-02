using Microsoft.Extensions.Configuration;
using QuizesManagement.BuisnessLogicLayer.Models.Gemini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace QuizesManagement.BuisnessLogicLayer.Services
{
    public class GeminiAiAdapter : IAiGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeminiAiAdapter(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GenerateJsonAsync(string prompt)
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
                throw new HttpRequestException($"Failed to generate content using Gemini AI. Status: {response.StatusCode}, Details: {errorContent}");
            }

            var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
            var generatedText = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

            if (string.IsNullOrWhiteSpace(generatedText))
            {
                throw new InvalidOperationException("Gemini AI returned an empty or invalid response.");
            }

            return generatedText.Trim().Replace("```json", "").Replace("```", "");
        }
    }
}
