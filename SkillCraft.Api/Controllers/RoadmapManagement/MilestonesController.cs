using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadmapMangement.BuisnessLogicLayer.Filters;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.BuisnessLogicLayer.Services;

namespace SkillCraft.Api.Controllers.RoadmapManagement
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Editor")]
    public class MilestonesController : ControllerBase
    {
        private readonly MilestonesService _milestonesService;

        public MilestonesController(MilestonesService milestonesService)
        {
            _milestonesService = milestonesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] EntityFilter filter)
        {
            var milestones = await _milestonesService.GetAll(filter);
            return Ok(milestones);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddMilestoneRequest addMilestoneRequest)
        {
            var milestone = await _milestonesService.Add(addMilestoneRequest);

            if (milestone is null)
            {
                return BadRequest();
            }
            return CreatedAtRoute("GetMilestoneAsync", new { id = milestone.Id }, milestone);
        }

        [HttpGet("{id}", Name = "GetMilestoneAsync")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var milestone = await _milestonesService.Get(id);

            if (milestone is null)
            {
                return BadRequest();
            }

            return Ok(milestone);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var milestone = await _milestonesService.DeleteAsync(id);

            if (milestone is null)
            {
                return BadRequest();
            }
            return Ok(milestone);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id,[FromBody] UpdateMilestoneRequest updateMilestoneRequest)
        {
            var milestone = await _milestonesService.UpdateAsync(id, updateMilestoneRequest);

            if (milestone is null)
            {
                return BadRequest();
            }

            return Ok(milestone);
        }


    }
}
