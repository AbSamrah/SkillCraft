using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.BuissnessLogicLayer.Services;

namespace SkillCraft.Api.Controllers.UsersManagement
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private RolesService _rolesService;
        public RolesController(RolesService rolesService) 
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _rolesService.GetAll();
            return Ok(roles);
        }
    }
}
