using BuissnessLogicLayer.Models;
using BuissnessLogicLayer.Services;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProfilesManagement.BuisnessLogicLayer.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IProfileService _profileService;

        public AuthController(AuthService authService, ITokenService tokenService, IProfileService profileService)
        {
            _authService = authService;
            _tokenService = tokenService;
            _profileService = profileService;
        }

        [HttpPost]
        [Route("signUp")]
        public async Task<IActionResult> SignUpAsync(UserSignUp userSignUp)
        {
            await _authService.SignUpAsync(userSignUp);
            return Ok(new { Message = "Verification email sent. Please check your inbox to complete registration." });
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            var user = await _authService.VerifyEmailAsync(email, token);
            await _profileService.Add(user.Id.ToString());

            var jwtToken = await _tokenService.GenerateToken(user);
            return Ok(new { Message = "Email verified successfully.", Token = jwtToken });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(UserLogin userLogin)
        {
            try
            {
                var user = await _authService.LoginAsync(userLogin);
                var token = await _tokenService.GenerateToken(user);
                return Ok(token);
            }
            catch (Exception ex) when (ex.Message == "Password is incorrect.")
            {
                return Conflict(new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Password is incorrect."
                });
            }
            catch (Exception ex) when (ex.Message == "User not found.")
            {
                return Conflict(new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "User not found."
                });
            }

        }

        [HttpPut]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest)
        {
            var user = await _authService.ChangePasswordAsync(changePasswordRequest);
            if (user is null)
            {
                return BadRequest();
            }

            return Ok(user);
        }
    }
}
