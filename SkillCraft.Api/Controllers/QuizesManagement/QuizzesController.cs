using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProfilesManagement.BuisnessLogicLayer.Services;
using QuizesManagement.BuisnessLogicLayer.Filters;
using QuizesManagement.BuisnessLogicLayer.Models;
using QuizesManagement.BuisnessLogicLayer.Services;
using QuizesManagement.DataAccessLayer.Models;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {
        private readonly IMultipleChoisesQuizService _mcqService;
        private readonly ITrueOrFalseQuizService _tfqService;
        private readonly IQuizService _quizService;
        private readonly IStrategyFactory _strategyFactory;
        private readonly IProfileService _profileService;

        public QuizzesController(
            IMultipleChoisesQuizService mcqService,
            ITrueOrFalseQuizService tfqService,
            IQuizService quizService,
            IStrategyFactory strategyFactory,
            IProfileService profileService)
        {
            _mcqService = mcqService;
            _tfqService = tfqService;
            _quizService = quizService;
            _strategyFactory = strategyFactory;
            _profileService = profileService;
        }

        #region --- General Quiz Endpoints ---

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllQuizzes([FromQuery] QuizFilter filter)
        {
            var excludedIds = new List<string>();
            
            if (User.IsInRole("User"))
            {
                var userId = User.FindFirst("id")?.Value;
                excludedIds = await _profileService.GetAllQuizzes(userId);
            }

            var quizzes = await _quizService.GetAll(filter, excludedIds);
            return Ok(quizzes);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteQuiz(string id)
        {
            var deletedQuiz = await _quizService.Delete(id);
            return Ok(deletedQuiz);
        }

        #endregion

        #region --- Multiple Choice Quiz (MCQ) Endpoints ---

        [HttpPost("mcq/manual")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> AddManualMcq(AddMultipleChoicesQuizRequest request)
        {
            var strategy = _strategyFactory.CreateStrategy("manual");
            var quiz = await _mcqService.Add(strategy, request);
            return CreatedAtRoute("GetMcqById", new { id = quiz.Id }, quiz);
        }

        [HttpPost("mcq/ai")]
        [Authorize]
        public async Task<IActionResult> AddAiMcq(AiQuizParameters parameters)
        {
            var userId = User.FindFirst("id")?.Value;
            if (User.IsInRole("User"))
            {
                var hasEnoughEnergy = await _profileService.CheckAndDeductEnergy(userId, 5);
                if (!hasEnoughEnergy)
                {
                    return StatusCode(429, new { Message = "Not enough energy to generate a roadmap. Please try again later." }); // 429 Too Many Requests
                }
            }
            var strategy = _strategyFactory.CreateStrategy("ai");
            var quiz = await _mcqService.Add(strategy, parameters);
            if (!User.IsInRole("Admin") && !User.IsInRole("Editor"))
            {
                quiz.Answer = null;
            }
            return CreatedAtRoute("GetMcqById", new {id = quiz.Id}, quiz);
        }

        [HttpGet("mcq/{id}", Name = "GetMcqById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMcqById(string id)
        {
            var quiz = await _mcqService.GetById(id);
            if (!User.IsInRole("Admin") && !User.IsInRole("Editor"))
            {
                quiz.Answer = null;
            }
            return Ok(quiz);
        }

        [HttpGet("mcq/answer/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckMcqAnswer([FromRoute] string id, [FromQuery] string answer)
        {
            var result = await _mcqService.CheckAnswer(id, answer);
            if (User.IsInRole("User") && result)
            {
                var userId = User.FindFirst("id")?.Value;
                await _profileService.AddQuiz(userId, id);
            }
            return Ok(result);
        }

        [HttpPut("mcq/{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> UpdateMcq([FromRoute] string id, [FromBody] EditMultipleChoicesQuizRequest request)
        {
            request.Id = id;
            var quiz = await _mcqService.Update(request);
            return Ok(quiz);
        }

        #endregion

        #region --- True/False Quiz (TFQ) Endpoints ---

        [HttpPost("tfq/manual")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> AddManualTfq(AddTrueOrFalseQuizRequest request)
        {
            var strategy = _strategyFactory.CreateStrategy("manual");
            var quiz = await _tfqService.Add(strategy, request);
            return CreatedAtRoute("GetTfqById", new { id = quiz.Id }, quiz);
        }

        [HttpPost("tfq/ai")]
        [Authorize]
        public async Task<IActionResult> AddAiTfq(AiQuizParameters parameters)
        {
            var userId = User.FindFirst("id")?.Value;
            if (User.IsInRole("User"))
            {
                var hasEnoughEnergy = await _profileService.CheckAndDeductEnergy(userId, 5);
                if (!hasEnoughEnergy)
                {
                    return StatusCode(429, new { Message = "Not enough energy to generate a roadmap. Please try again later." }); // 429 Too Many Requests
                }
            }
            var strategy = _strategyFactory.CreateStrategy("ai");
            var quiz = await _tfqService.Add(strategy, parameters);
            if (!User.IsInRole("Admin") && !User.IsInRole("Editor"))
            {
                quiz.Answer = null;
            }
            return CreatedAtRoute("GetTfqById", new { id = quiz.Id }, quiz);
        }

        [HttpGet("tfq/{id}", Name = "GetTfqById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTfqById(string id)
        {
            var quiz = await _tfqService.GetById(id);
            if (!User.IsInRole("Admin") && !User.IsInRole("Editor"))
            {
                quiz.Answer = null;
            }
            return Ok(quiz);
        }

        [HttpGet("tfq/answer/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckTfqAnswer([FromRoute] string id, [FromQuery] bool answer)
        {
            var result = await _tfqService.CheckAnswer(id, answer);
            if (User.IsInRole("User") && result)
            {
                var userId = User.FindFirst("id")?.Value;
                await _profileService.AddQuiz(userId, id);
            }

            return Ok(result);
        }

        [HttpPut("tfq/{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> UpdateTfq([FromRoute] string id, [FromBody] EditTrueOrFalseQuizRequest request)
        {
            request.Id = id;
            var quiz = await _tfqService.Update(request);
            return Ok(quiz);
        }

        #endregion
    }
}

