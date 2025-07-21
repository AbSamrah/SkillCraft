using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizesManagement.BuisnessLogicLayer.Models;
using QuizesManagement.BuisnessLogicLayer.Services;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.BuisnessLogicLayer.Services;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultipleChoicesQuizController : ControllerBase
    {
        private readonly IMultipleChoisesQuizService _quizService;

        public MultipleChoicesQuizController(IMultipleChoisesQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var quizes = await _quizService.GetAll();
            return Ok(quizes);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddMultipleChoicesQuizRequest quizRequest)
        {
            var quiz = await _quizService.Add(quizRequest);

            if (quiz is null)
            {
                return BadRequest();
            }
            return Ok(quiz);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(string id)
        {
            var quiz = await _quizService.GetById(id);
            quiz.Answer = null;
            if (quiz is null)
            {
                return BadRequest("Something went wrong.");
            }

            return Ok(quiz);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var quiz = await _quizService.Delete(id);

            if (quiz is null)
            {
                return BadRequest("Quiz not found.");
            }
            return Ok(quiz);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(EditMultipleChoicesQuizRequest quizRequest)
        {
            var quiz = await _quizService.Update(quizRequest);

            if (quiz is null)
            {
                return BadRequest();
            }

            return Ok(quiz);
        }

        [HttpPut]
        [Route("/answer/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckAnswer([FromRoute] string id, [FromBody] string answer)
        {
            var result = await _quizService.CheckAnswer(id, answer);
            return Ok(result);
        }
    }
}
