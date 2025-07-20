using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.BuisnessLogicLayer.Services;

namespace SkillCraft.Api.Controllers.RoadmapManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoadmapsController : ControllerBase
    {
        private readonly RoadmapsService _roadmapsService;

        public RoadmapsController(RoadmapsService roadmapsService)
        {
            _roadmapsService = roadmapsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var roadmaps = await _roadmapsService.GetAll();
            return Ok(roadmaps);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddRoadmapRequest addRoadmapRequest)
        {
            var roadmap = await _roadmapsService.Add(addRoadmapRequest);

            if (roadmap is null)
            {
                return BadRequest();
            }
            return Ok(roadmap);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var roadmap = await _roadmapsService.Get(id);

            if (roadmap is null)
            {
                return BadRequest();
            }

            return Ok(roadmap);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var roadmap = await _roadmapsService.DeleteAsync(id);

            if (roadmap is null)
            {
                return BadRequest();
            }
            return Ok(roadmap);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute]string id,[FromBody] UpdateRoadmapRequest updateRoadmapRequest)
        {
            var roadmap = await _roadmapsService.UpdateAsync(id, updateRoadmapRequest);

            if (roadmap is null)
            {
                return BadRequest();
            }

            return Ok(roadmap);
        }

    }
}
