using CampusCourses.Data.DTO.Report;
using CampusCourses.Data.Entities.Enums;

namespace CampusCourses.Services.IServices
{
    public interface IReportService
    {
        public Task<List<TeacherReportRecordModel>> reportGeneate(Guid userId, Semester? semester, List<Guid>? idsGroup);
    }
}
