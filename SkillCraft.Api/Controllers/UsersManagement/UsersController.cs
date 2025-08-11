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

        [HttpGet("{id:guid}", Name ="GetUserAsync")]
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
            var newUser = await _usersService.AddAsync(user);
            if(newUser.Role == "User")
            {
                await _profileService.Add(newUser.Id.ToString());
            }
            if (newUser is null)
            {
                return NotFound();
            }
            return CreatedAtRoute("GetUserAsync", new { id = newUser.Id }, newUser);
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
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            if (!Guid.TryParse(id, out Guid userId))
            {
                return BadRequest(new { message = "Invalid user ID format." });
            }

            try
            {
                var (user, profileAction) = await _usersService.UpdateAsync(userId, updateUserRequest);

                switch (profileAction)
                {
                    case ProfileAction.CREATE_PROFILE:
                        await _profileService.Add(user.Id.ToString());
                        break;
                    case ProfileAction.DELETE_PROFILE:
                        await _profileService.Remove(user.Id.ToString());
                        break;
                    case ProfileAction.NONE:
                        break;
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
