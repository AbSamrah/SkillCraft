using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfilesManagement.BuisnessLogicLayer.Services;
using RoadmapMangement.BuisnessLogicLayer.Services;
using System.Security.Claims;

namespace SkillCraft.Api.Controllers.ProfileManagement
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IRoadmapsService _roadmapsService;


        public ProfileController(IProfileService profileService, IRoadmapsService roadmapsService)
        {
            _profileService = profileService;
            _roadmapsService = roadmapsService;
        }
        [HttpGet("MyRoadmaps")]
        public async Task<IActionResult> GetProfileRoadmaps()
        {
            var userId = User.FindFirst("id")?.Value;
            var roadmaps = await _profileService.GetAllRoadmaps(userId);
            
            return Ok(roadmaps);
        }

        [HttpGet("FinishedRoadmaps")]
        public async Task<IActionResult> GetRoadmaps([FromQuery] bool finished)
        {
            var userId = User.FindFirst("id")?.Value;
            var roadmaps = await _profileService.GetRoadmaps(userId, finished);
            return Ok(roadmaps);
        }

        [HttpGet("FinishedSteps/{roadmapId}")]
        public async Task<IActionResult> GetFinishedSteps(string roadmapId)
        {
            var userId = User.FindFirst("id")?.Value;
            var roadmap = await _roadmapsService.Get(roadmapId);
            var milestones = roadmap.Milestones;
            var stepsIds = new List<string>();
            foreach (var milestone in milestones)
            {
                foreach (var step in milestone.Steps)
                {
                    stepsIds.Add(step.Id);
                }
            }
            var finishedSteps = await _profileService.GetFinishedStep(userId, stepsIds);
            return Ok(finishedSteps);
        }

        [HttpPut("FinishSteps")]
        public async Task<IActionResult> FinishSteps(List<string> stepsIds)
        {
            var userId = User.FindFirst("id")?.Value;
            await _profileService.FinishSteps(userId, stepsIds);
            return Ok();
        }

        [HttpPut("UnFinishSteps")]
        public async Task<IActionResult> UnFinishSteps(List<string> stepsIds)
        {
            var userId = User.FindFirst("id")?.Value;
            await _profileService.UnFinishSteps(userId, stepsIds);
            return Ok();
        }

        [HttpPut("Roadmaps/{roadmapId}")]
        public async Task<IActionResult> ChangeRoadmapStatus(string roadmapId, bool finish = true)
        {
            var userId = User.FindFirst("id")?.Value;
            await _profileService.ChangeRoadmapStatus(userId, roadmapId, finish);
            return Ok();
        }

        [HttpPut("AddRoadmap/{roadmapId}")]
        public async Task<IActionResult> AddRoadmap(string roadmapId)
        {
            var userId = User.FindFirst("id")?.Value;
            await _profileService.AddRoadmap(userId, roadmapId);
            return Ok();
        }

        [HttpPut("RemoveRoadmap/{roadmapId}")]
        public async Task<IActionResult> RemoveRoadmap(string roadmapId)
        {
            var userId = User.FindFirst("id")?.Value;
            await _profileService.RemoveRoadmap(userId, roadmapId);
            return Ok();
        }

        [HttpGet("CheckRoadmap/{roadmapId}")]
        public async Task<IActionResult> CheckRoadmap(string roadmapId)
        {
            var userId = User.FindFirst("id")?.Value;
            var result = await _profileService.CheckRoadmap(userId, roadmapId);
            return Ok(result);
        }

        [HttpGet("energy/{id}")]
        public async Task<IActionResult> GetEnergy([FromRoute] string id)
        {
            int energy = await _profileService.GetEnergy(id);
            return Ok(energy);
        }
    }
}
