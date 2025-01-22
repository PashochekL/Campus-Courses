using CampusCourses.Data.DTO.Account;
using CampusCourses.Data.DTO.User;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusCourses.Data.Entities;

namespace CampusCourses.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("/users")]
        [Authorize]
        [ProducesResponseType(typeof(List<UserShortModel>), 200)]
        [ProducesResponseType(typeof(Error), 401)]
        [ProducesResponseType(typeof(Error), 403)]
        [ProducesResponseType(typeof(Error), 500)]
        public async Task<ActionResult<UserShortModel>> getListUser()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    List<UserShortModel> userShortModel = await _service.getListAllUser(userId);

                    return Ok(new { userShortModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpGet("/roles")]
        [Authorize]
        [ProducesResponseType(typeof(UserRolesModel), 200)]
        [ProducesResponseType(typeof(Error), 401)]
        [ProducesResponseType(typeof(Error), 500)]
        public async Task<IActionResult> getRoles()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {

                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    UserRolesModel userRolesModel = await _service.getUserRoles(userId);

                    return Ok(new { userRolesModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }
    }
}
