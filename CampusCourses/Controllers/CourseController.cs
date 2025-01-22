using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.DTO.Group;
using CampusCourses.Data.DTO.Student;
using CampusCourses.Data.Entities.Enums;
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

        [HttpPost("/courses/{id}/sign-up")]
        [Authorize]
        public async Task<IActionResult> signUpCourse(Guid id)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    StudentViewModel studentViewModel = await _service.signUpCampusCourse(id, userId);

                    return Ok(new { studentViewModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpPost("/courses/{id}/status")]
        [Authorize]
        public async Task<IActionResult> editStatus(Guid id, EditCourseStatusModel editCourseStatusModel)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    CampusCourseDetailsModel campusCourseDetailsModel = await _service.editCourseStatus(id, editCourseStatusModel, userId);

                    return Ok(new { campusCourseDetailsModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpPost("/courses/{id}/student-status/{studentId}")]
        [Authorize]
        public async Task<IActionResult> editStatusSignedStudent(Guid id, Guid studentId, [FromBody] EditCourseStudentStatusModel EditCourseStudentStatusModel)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    CampusCourseDetailsModel campusCourseDetailsModel = await _service.editStatusStudent(id, studentId, userId, EditCourseStudentStatusModel);

                    return Ok(new { campusCourseDetailsModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

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

        [HttpPost("/courses/{id}/marks/{studentId}")]
        [Authorize]
        public async Task<IActionResult> editStidentMark(Guid id, Guid studentId, [FromBody] EditCourseStudentMarkModel editCourseStudentMarkModel)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    CampusCourseDetailsModel campusCourseDetailsModel = await _service.editMark(id, studentId, editCourseStudentMarkModel, userId);

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

        [HttpPut("/courses/{id}/requirements-and-annotations")]
        [Authorize]
        public async Task<IActionResult> editCourseRequirementsAndAnnotations(Guid id, [FromBody] EditCampusCourseRequirementsAndAnnotationsModel editModel)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    CampusCourseDetailsModel campusCourseDetailsModel = await _service.editAnnotations(id, editModel, userId);

                    return Ok(new { campusCourseDetailsModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpPut("/courses/{id}")]
        [Authorize]
        public async Task<IActionResult> editCampusCourse(Guid id, [FromBody] EditCampusCourseModel editModel)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    CampusCourseDetailsModel campusCourseDetailsModel = await _service.editCourse(id, editModel, userId);

                    return Ok(new { campusCourseDetailsModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpDelete("/courses/{id}")]
        [Authorize]
        public async Task<IActionResult> deleteCampusCourse(Guid id)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    await _service.deleteCourse(id, userId);

                    return Ok();
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpPost("/course/{id}/teachers")]
        [Authorize]
        public async Task<IActionResult> addCampusTeacher(Guid id, [FromBody] AddTeacherToCourseModel addTeacherToCourseModel)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    CampusCourseDetailsModel CampusCourseDetailsModel = await _service.addTeacher(id, addTeacherToCourseModel, userId);

                    return Ok(new { CampusCourseDetailsModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpGet("/courses/my")]
        [Authorize]
        public async Task<IActionResult> getCampusCourseMy()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    List<CampusCoursePreviewModel> courseList = await _service.getMyCourse(userId);

                    return Ok(new { courseList });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpGet("/courses/teaching")]
        [Authorize]
        public async Task<IActionResult> getCampusCourseTeaching()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    List<CampusCoursePreviewModel> courseList = await _service.getCourseTeaching(userId);

                    return Ok(new { courseList });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }

        [HttpGet("/courses/list")]
        public async Task<ActionResult<CampusCoursePreviewModel>> getCampusCourseList([FromQuery] Sort? sort = null, string? search = null, bool? hasPlacesAndOpen = null,
            [FromQuery] Semester? semester = null, int page = 1, int size = 10)
        {
            List<CampusCoursePreviewModel> campusCoursesPreviewModel = await _service.getCourseList(page, size, sort, search, hasPlacesAndOpen, semester);
            
            return Ok(new { campusCoursesPreviewModel });
        }
    }
}
