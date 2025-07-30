using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizesManagement.BuisnessLogicLayer.Models;
using QuizesManagement.BuisnessLogicLayer.Services;

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

        public QuizzesController(
            IMultipleChoisesQuizService mcqService,
            ITrueOrFalseQuizService tfqService,
            IQuizService quizService,
            IStrategyFactory strategyFactory)
        {
            _mcqService = mcqService;
            _tfqService = tfqService;
            _quizService = quizService;
            _strategyFactory = strategyFactory;
        }

        #region --- General Quiz Endpoints ---

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllQuizzes()
        {
            var quizzes = await _quizService.GetAll();
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
        [AllowAnonymous]
        public async Task<IActionResult> AddAiMcq(AiQuizParameters parameters)
        {
            var strategy = _strategyFactory.CreateStrategy("ai");
            var quiz = await _mcqService.Add(strategy, parameters);
            return CreatedAtRoute("GetMcqById", new {id = quiz.Id}, quiz);
        }

        [HttpGet("mcq/{id}", Name = "GetMcqById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMcqById(string id)
        {
            var quiz = await _mcqService.GetById(id);
            quiz.Answer = null;
            return Ok(quiz);
        }

        [HttpGet("mcq/answer/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckMcqAnswer([FromRoute] string id, [FromQuery] string answer)
        {
            var result = await _mcqService.CheckAnswer(id, answer);
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
        [AllowAnonymous]
        public async Task<IActionResult> AddAiTfq(AiQuizParameters parameters)
        {
            var strategy = _strategyFactory.CreateStrategy("ai");
            var quiz = await _tfqService.Add(strategy, parameters);
            return CreatedAtRoute("GetTfqById", new { id = quiz.Id }, quiz);
        }

        [HttpGet("tfq/{id}", Name = "GetTfqById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTfqById(string id)
        {
            var quiz = await _tfqService.GetById(id);
            quiz.Answer = null;
            return Ok(quiz);
        }

        [HttpGet("tfq/answer/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckTfqAnswer([FromRoute] string id, [FromQuery] bool answer)
        {
            var result = await _tfqService.CheckAnswer(id, answer);
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

