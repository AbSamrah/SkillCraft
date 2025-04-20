using BuissnessLogicLayer.Filters;
using BuissnessLogicLayer.Models;
using BuissnessLogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserRequest user)
        {
            user = await _usersService.AddAsync(user);
            if (user is null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody]UpdateUserRequest updateUserRequest)
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
