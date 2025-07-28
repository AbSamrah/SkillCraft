using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly Func<string, IRoadmapCreationStrategy> _strategyFactory;

        public RoadmapsController(IRoadmapsService roadmapsService, Func<string, IRoadmapCreationStrategy> strategyFactory)
        {
            _roadmapsService = roadmapsService;
            _strategyFactory = strategyFactory;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var roadmaps = await _roadmapsService.GetAll();
            return Ok(roadmaps);
        }

        /// <summary>
        /// Creates a roadmap manually.
        /// </summary>
        [HttpPost("manual")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> AddManualAsync(AddRoadmapRequest addRoadmapRequest)
        {
            var strategy = _strategyFactory("manual");
            var roadmap = await _roadmapsService.Add(strategy, addRoadmapRequest);
            return CreatedAtRoute("GetRoadmapAsync", new { id = roadmap.Id }, roadmap);
        }

        /// <summary>
        /// Creates a roadmap using AI.
        /// </summary>
        [HttpPost("ai")]
        //[Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> AddAiAsync([FromBody] AiRoadmapParameters aiParams)
        {
            var strategy = _strategyFactory("ai");
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
