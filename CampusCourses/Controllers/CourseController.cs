using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.DTO.Group;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusCourses.Controllers
{
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        public CourseController(ICourseService service)
        {
            _service = service;
        }

        [HttpGet("/courses/{id}/details")]
        [Authorize]
        public async Task<IActionResult> getCampusCourseDetails(Guid id)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    CampusCourseDetailsModel campusCourseDetailsModel = await _service.getCampusDetails(id, userId);

                    return Ok(new { campusCourseDetailsModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        /*[HttpPost("/courses/{id}/sign-up")]
        [Authorize]
        public async Task<IActionResult> signUpCourse(Guid id)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    await _service.signUpCampusCourse(id, userId);

                    return Ok();
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }*/

        [HttpPost("/courses/{id}/notifications")]
        [Authorize]
        public async Task<IActionResult> createCampusNotification(Guid id, [FromBody] AddCampusCourseNotificationModel createNotification)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    CampusCourseDetailsModel campusCourseDetailsModel = await _service.createNotification(id, createNotification, userId);

                    return Ok(new { campusCourseDetailsModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpPost("/groups/{groupId}")]
        [Authorize]
        public async Task<IActionResult> createCampusGroup(Guid groupId, [FromBody] CreateCampusCourseModel createCampusCourseModel)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    CampusCoursePreviewModel campusCoursePreviewModel = await _service.createNewCourse(groupId, createCampusCourseModel, userId);

                    return Ok(new { campusCoursePreviewModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }
    }
}
