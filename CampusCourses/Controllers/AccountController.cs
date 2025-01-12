using CampusCourses.Data.DTO.Account;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CampusCourses.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> registerNewUser([FromBody] UserRegisterModel userRegisterModel)
        {
            string tokenAutorize = await _service.registerUser(userRegisterModel);

            return Ok(new { token = tokenAutorize });
        }

        [HttpPost("login")]
        public async Task<IActionResult> autorizeUser([FromBody] UserLoginModel userLoginModel)
        {
            string tokenAutorize = await _service.autorizeUser(userLoginModel);

            return Ok(new { token = tokenAutorize });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> logoutUser()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (userIdClaim != null && authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    await _service.userLogout(userId, token);

                    return Ok(new { Message = "Выход выполнен успешно" });
                }
            }

            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileModel>> getProfile()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {

                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    UserProfileModel userModelDTO = await _service.getUserProfile(userId);

                    return Ok(new { userModelDTO });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileModel>> eitUserProfile([FromBody] EditUserProfileModel editUserProfileModel)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    UserProfileModel userModelDTO = await _service.editProfile(userId, editUserProfileModel);

                    return Ok(new { userModelDTO });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }
    }
}
