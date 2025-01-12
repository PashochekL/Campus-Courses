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

        [HttpPost("groups/{groupId}")]
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
