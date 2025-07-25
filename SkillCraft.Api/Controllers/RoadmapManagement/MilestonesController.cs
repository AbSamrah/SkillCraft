using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.BuisnessLogicLayer.Services;

namespace SkillCraft.Api.Controllers.RoadmapManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilestonesController : ControllerBase
    {
        private readonly MilestonesService _milestonesService;

        public MilestonesController(MilestonesService milestonesService)
        {
            _milestonesService = milestonesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var milestones = await _milestonesService.GetAll();
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
            //return CreatedAtAction(nameof(GetAsync), new { id = milestone.Id }, milestone);
            return Ok(milestone);
        }

        [HttpGet("{id}")]
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
