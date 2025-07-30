using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public RoadmapsController(IRoadmapsService roadmapsService, IStrategyFactory strategyFactory)
        {
            _roadmapsService = roadmapsService;
            _strategyFactory = strategyFactory;
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
        public async Task<IActionResult> AddAiAsync([FromBody] AiRoadmapParameters aiParams)
        {
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
