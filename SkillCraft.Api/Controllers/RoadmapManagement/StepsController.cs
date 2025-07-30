using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadmapMangement.BuisnessLogicLayer.Filters;
using RoadmapMangement.BuisnessLogicLayer.Models;
using RoadmapMangement.BuisnessLogicLayer.Services;

namespace SkillCraft.Api.Controllers.RoadmapManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class StepsController : ControllerBase
    {
        private readonly StepsService _stepsService;

        public StepsController(StepsService stepsService)
        {
            _stepsService = stepsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] EntityFilter filter)
        {
            var steps = await _stepsService.GetAll(filter);
            return Ok(steps);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddStepRequest addStepRequest)
        {
            var step = await _stepsService.Add(addStepRequest); 
            
            if (step is null)
            {
                return BadRequest();
            }
            return CreatedAtRoute("GetStepAsync", new { id = step.Id }, step);
        }

        [HttpGet("{id}", Name = "GetStepAsync")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var step = await _stepsService.Get(id);

            if (step is null)
            {
                return NotFound();
            }

            return Ok(step);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var step = await _stepsService.DeleteAsync(id);

            if (step is null)
            {
                return NotFound();
            }
            return Ok(step);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateStepRequest updateStepRequest)
        {
            var step = await _stepsService.UpdateAsync(id, updateStepRequest);

            if (step is null)
            {
                return NotFound();
            }

            return Ok(step);
        }
    }
}
