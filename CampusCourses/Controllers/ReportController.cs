using CampusCourses.Data.DTO.Report;
using CampusCourses.Data.DTO.User;
using CampusCourses.Data.Entities.Enums;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusCourses.Data.Entities;

namespace CampusCourses.Controllers
{
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _service;
        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpGet("/report")]
        [Authorize]
        [ProducesResponseType(typeof(List<TeacherReportRecordModel>), 200)]
        [ProducesResponseType(typeof(Error), 401)]
        [ProducesResponseType(typeof(Error), 403)]
        [ProducesResponseType(typeof(Error), 500)]
        public async Task<ActionResult<TeacherReportRecordModel>> getReport([FromQuery] Semester? semester, [FromQuery] List<Guid>? idsGroup)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null)
            {
                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    List<TeacherReportRecordModel> teacherReportRecordModel = await _service.reportGeneate(userId, semester, idsGroup);

                    return Ok(new { teacherReportRecordModel });
                }
            }
            throw new UnauthorizedException("Пользователь не авторизован");
        }
    }
}
