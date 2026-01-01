using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotocopyConnectedAPI.Services.Interfaces;
using PhotocopyConnectedAPI.Models.DTOs.Request.Account;
using PhotocopyConnectedAPI.Configurations;
using System.Security.Claims;
using PhotocopyConnectedAPI.Models.DTOs.Request.Profile;
//using PhotocopyConnectedAPI.Models;

namespace PhotocopyConnectedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly JwtConfig _jwtConfig;
        public AccountController(IAccountService accountService, JwtConfig jwtConfig)
        {
            _accountService = accountService;
            _jwtConfig = jwtConfig;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register ([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.RegisterAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login ([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.LoginAsync(request);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            Response.Cookies.Append("accessToken", result.Data!.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpirationMinutes)
            });

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken ([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.RefreshTokenAsync(request);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            Response.Cookies.Append("accessToken", result.Data!.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpirationMinutes)
            });

            return Ok(result);
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var result = await _accountService.LogoutAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var result = await _accountService.GetProfileAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPut("user")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var result = await _accountService.GetProfileAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        




        
    }
}