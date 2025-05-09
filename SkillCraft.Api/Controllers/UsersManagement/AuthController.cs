﻿using BuissnessLogicLayer.Models;
using BuissnessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("signUp")]
        public async Task<IActionResult> SignUpAsync(UserSignUp userSignUp)
        {
            try
            {
                var token = await _authService.SignUpAsync(userSignUp);
                return Ok(token);
            }
            catch (Exception ex) when (ex.Message == "Email already exists.")
            {
                return Conflict(new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Email is already registered."
                });
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(UserLogin userLogin)
        {
            try
            {
                var token = await _authService.LoginAsync(userLogin);
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
