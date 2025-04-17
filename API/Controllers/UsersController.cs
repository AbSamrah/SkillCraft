using BuissnessLogicLayer.Models;
using BuissnessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
        public async Task<IActionResult> GetAll(string? email = null, string? firstName = null, string? lastName=null)
        {
            var users = await _usersService.GetAllAsync(email, firstName, lastName);
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _usersService.GetByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("email")]
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
            return Ok(user);
        }
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _usersService.DeleteAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UserDto user)
        {
            user = await _usersService.UpdateAsync(user);
            if (user is null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
    }
}
