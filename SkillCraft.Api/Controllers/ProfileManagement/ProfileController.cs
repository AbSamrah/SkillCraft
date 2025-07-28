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
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IRoadmapsService _roadmapsService;


        public ProfileController(IProfileService profileService, IRoadmapsService roadmapsService)
        {
            _profileService = profileService;
            _roadmapsService = roadmapsService;
        }
        [HttpGet("{userId}/AllRoadmaps")]
        public async Task<IActionResult> GetProfileRoadmaps(string userId)
        {
            var roadmaps = await _profileService.GetAllRoadmaps(userId);
            return Ok(roadmaps);
        }

        [HttpGet("{userId}/Roadmaps")]
        public async Task<IActionResult> GetRoadmaps(string userId, [FromQuery] bool finished)
        {
            var roadmaps = await _profileService.GetRoadmaps(userId, finished);
            return Ok(roadmaps);
        }

        [HttpGet("{userId}/FinishedSteps/{roadmapId}")]
        public async Task<IActionResult> GetFinishedSteps(string userId, string roadmapId)
        {
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

        [HttpPut("{userId}/FinishSteps")]
        public async Task<IActionResult> FinishSteps(string userId, List<string> stepsIds)
        {
            await _profileService.FinishSteps(userId, stepsIds);
            return Ok();
        }

        [HttpPut("{userId}/UnFinishSteps")]
        public async Task<IActionResult> UnFinishSteps(string userId, List<string> stepsIds)
        {
            await _profileService.UnFinishSteps(userId, stepsIds);
            return Ok();
        }

        [HttpPut("{userId}/Roadmaps/{roadmapId}")]
        public async Task<IActionResult> ChangeRoadmapStatus(string userId, string roadmapId, bool finish = true)
        {
            await _profileService.ChangeRoadmapStatus(userId, roadmapId, finish);
            return Ok();
        }

        [HttpPut("{userId}/AddRoadmap/{roadmapId}")]
        public async Task<IActionResult> AddRoadmap(string userId, string roadmapId)
        {
            await _profileService.AddRoadmap(userId, roadmapId);
            return Ok();
        }

        [HttpPut("{userId}/RemoveRoadmap/{roadmapId}")]
        public async Task<IActionResult> RemoveRoadmap(string userId, string roadmapId)
        {
            await _profileService.RemoveRoadmap(userId, roadmapId);
            return Ok();
        }


        [HttpGet("{userId}/CheckRoadmap/{roadmapId}")]
        public async Task<IActionResult> CheckRoadmap(string userId, string roadmapId)
        {
            var result = await _profileService.CheckRoadmap(userId, roadmapId);
            return Ok(result);
        }
    }
}
