using BuissnessLogicLayer.Filters;
using BuissnessLogicLayer.Models;
using BuissnessLogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowReactApp")]
    public class UsersController : Controller
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserFilter userFilter)
        {
            var users = await _usersService.GetAllAsync(userFilter);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _usersService.GetByIdAsync(id);
            if (user is null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _usersService.GetByEmailAsync(email);
            if (user is null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserRequest user)
        {
            user = await _usersService.AddAsync(user);
            if (user is null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _usersService.DeleteAsync(id);
            if (user is null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest updateUserRequest)
        {
            updateUserRequest = await _usersService.UpdateAsync(updateUserRequest);
            if (updateUserRequest is null)
            {
                return BadRequest();
            }
            return Ok(updateUserRequest);
        }
    }
}
