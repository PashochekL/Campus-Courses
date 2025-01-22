using CampusCourses.Data;
using CampusCourses.Data.DTO.Report;
using CampusCourses.Data.Entities.Enums;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CampusCourses.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDBContext _dbContext;
        private readonly HelperService _helperService;

        public ReportService(AppDBContext dbContext, HelperService helperService)
        {
            _dbContext = dbContext;
            _helperService = helperService;
        }

        public async Task<List<TeacherReportRecordModel>> reportGeneate(Guid userId, Semester? semester, List<Guid>? idsGroup)
        {
            var account = await _helperService.checkAutorize(userId);
            if (!account.isAdmin) throw new ForbiddenException("У вас недостаточно прав");

            var findCourses = _dbContext.Teachers
           .Include(t => t.Course)
               .ThenInclude(c => c.Group)
           .Include(t => t.Course)
               .ThenInclude(c => c.Teachers)
           .Include(t => t.Account)
           .Where(t => t.mainTeacher)
           .AsQueryable();

            if (semester.HasValue) findCourses = findCourses.Where(t => t.Course.Semester == semester.Value);

            if (idsGroup is { Count: > 0 }) findCourses = findCourses.Where(t => t.Course.GroupId == idsGroup.First());

            var coursesList = await findCourses.ToListAsync();

            if (coursesList.Count == 0 && idsGroup is { Count: > 0 }) throw new BadRequestException("Переданные id групп не существуют");

            var reportList = await findCourses.GroupBy(t => new { t.UserId, t.Account.FullName })
            .Select(teacherGroup => new TeacherReportRecordModel
            {
                fullName = teacherGroup.Key.FullName,
                id = teacherGroup.Key.UserId,
                campusGroupReports = teacherGroup
                    .GroupBy(t => new { t.Course.Group.Id, t.Course.Group.Name })
                    .Select(groupByGroup => new CampusGroupReportModel
                    {
                        id = groupByGroup.Key.Id,
                        name = groupByGroup.Key.Name,
                        averagePassed = groupByGroup.Sum(c => c.Course.Students.Count(s => s.FinalResult == StudentMarks.Passed))
                            / (double)(groupByGroup.Sum(c => c.Course.Students.Count) == 0 ? 1 : groupByGroup.Sum(c => c.Course.Students.Count)),

                        averageFailed = groupByGroup.Sum(c => c.Course.Students.Count(s => s.FinalResult == StudentMarks.Failed))
                            / (double)(groupByGroup.Sum(c => c.Course.Students.Count) == 0 ? 1 : groupByGroup.Sum(c => c.Course.Students.Count)),
                    })
                    .ToList()
            }).OrderBy(r => r.fullName).ToListAsync();

            return reportList;
        }
    }
}
