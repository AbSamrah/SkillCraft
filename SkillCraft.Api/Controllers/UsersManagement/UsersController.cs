using BuissnessLogicLayer.Filters;
using BuissnessLogicLayer.Models;
using BuissnessLogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProfilesManagement.BuisnessLogicLayer.Services;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowReactApp")]
    public class UsersController : Controller
    {
        private readonly UsersService _usersService;
        private readonly IProfileService _profileService;

        public UsersController(UsersService usersService, IProfileService profileService)
        {
            _usersService = usersService;
            _profileService = profileService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserFilter userFilter)
        {
            var users = await _usersService.GetAllAsync(userFilter);
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _usersService.GetByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("{email:alpha}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _usersService.GetByEmailAsync(email);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserRequest user)
        {   
            user = await _usersService.AddAsync(user);
            if(user.Role == "User")
            {
                await _profileService.Add(user.Id.ToString());
            }
            if (user is null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _usersService.DeleteAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            await _profileService.Remove(id.ToString());
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest updateUserRequest)
        {
            updateUserRequest = await _usersService.UpdateAsync(updateUserRequest);
            if (updateUserRequest is null)
            {
                return NotFound();
            }
            return Ok(updateUserRequest);
        }
    }
}
