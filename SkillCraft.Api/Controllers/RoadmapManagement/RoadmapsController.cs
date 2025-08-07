using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfilesManagement.BuisnessLogicLayer.Services;
using QuizesManagement.DataAccessLayer.Models;
using RoadmapMangement.BuisnessLogicLayer.Filters;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.BuisnessLogicLayer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillCraft.Api.Controllers.RoadmapManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoadmapsController : ControllerBase
    {
        private readonly IRoadmapsService _roadmapsService;
        private readonly IStrategyFactory _strategyFactory;
        private readonly IProfileService _profileService;
        private readonly StepsService _stepsService;
        public RoadmapsController(IRoadmapsService roadmapsService, IStrategyFactory strategyFactory, IProfileService profileService, StepsService stepsService)
        {
            _roadmapsService = roadmapsService;
            _strategyFactory = strategyFactory;
            _profileService = profileService;
            _stepsService = stepsService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] EntityFilter filter)
        {
            var roadmaps = await _roadmapsService.GetAll(filter);
            return Ok(roadmaps);
        }

        [HttpPost("manual")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> AddManualAsync(AddRoadmapRequest addRoadmapRequest)
        {
            var strategy = _strategyFactory.CreateStrategy("manual");
            var roadmap = await _roadmapsService.Add(strategy, addRoadmapRequest);
            return CreatedAtRoute("GetRoadmapAsync", new { id = roadmap.Id }, roadmap);
        }

        [HttpPost("ai")]
        [Authorize]
        public async Task<IActionResult> AddAiAsync([FromBody] PromptParameter parameter)
        {
            var userId = User.FindFirst("id")?.Value;
            if (User.IsInRole("User"))
            {
                var hasEnoughEnergy = await _profileService.CheckAndDeductEnergy(userId, 10);
                if (!hasEnoughEnergy)
                {
                    return StatusCode(429, new { Message = "Not enough energy to generate a roadmap. Please try again later." }); // 429 Too Many Requests
                }
            }
            AiRoadmapParameters aiParams = new AiRoadmapParameters();
            aiParams.Prompt = parameter.prompt;
            List<string> stepsIds = new List<string>();
            if (!User.IsInRole("Admin") && !User.IsInRole("Editor"))
            {
                stepsIds = await _profileService.GetAllSteps(userId);
            }
            var steps = await _stepsService.GetMany(stepsIds);
            aiParams.CompletedSteps = [.. steps];
            var strategy = _strategyFactory.CreateStrategy("ai");
            var roadmap = await _roadmapsService.Add(strategy, aiParams);
            return CreatedAtRoute("GetRoadmapAsync", new { id = roadmap.Id }, roadmap);
        }

        [HttpGet("{id}", Name = "GetRoadmapAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(string id)
        {
            var roadmap = await _roadmapsService.Get(id);
            return Ok(roadmap);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var roadmap = await _roadmapsService.DeleteAsync(id);
            return Ok(roadmap);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateRoadmapRequest updateRoadmapRequest)
        {
            var roadmap = await _roadmapsService.UpdateAsync(id, updateRoadmapRequest);
            return Ok(roadmap);
        }
    }
}
