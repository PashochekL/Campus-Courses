namespace CampusCourses.Data.DTO.Report
{
    public class TeacherReportRecordModel
    {
        public string? fullName { get; set; }
        public Guid id { get; set; }
        public ICollection<CampusGroupReportModel> campusGroupReports { get; set; } = new List<CampusGroupReportModel>();
    }
}
