using CampusCourses.Data;
using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.DTO.Group;
using CampusCourses.Data.Entities;
using CampusCourses.Data.Entities.Enums;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace CampusCourses.Services
{
    public class GroupService : IGroupService
    {
        private readonly AppDBContext _dbContext;
        private readonly HelperService _helperService;

        public GroupService(AppDBContext dbContext, HelperService helperService)
        {
            _dbContext = dbContext;
            _helperService = helperService;
        }

        public async Task<List<CampusGroupModel>> getListAllGroups(Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);

            var groups = await _dbContext.Groups.OrderBy(gr => gr.Name)
                .Select(group => new CampusGroupModel()
                {
                    id = group.Id,
                    name = group.Name
                }).ToListAsync();

            return groups;
        }

        public async Task<CampusGroupModel> createGroup(CreateCampusGroupModel createCampusGroupModel, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);

            if (!account.isAdmin) throw new ForbiddenException("У вас нет прав администратора");

            var newGroup = new Group
            {
                Name = createCampusGroupModel.name,
                CreatedDate = DateTime.UtcNow
            };
            _dbContext.Add(newGroup);
            await _dbContext.SaveChangesAsync();

            CampusGroupModel campusGroupModel = new CampusGroupModel()
            {
                name = createCampusGroupModel.name,
                id = newGroup.Id
            };
            return campusGroupModel;
        }

        public async Task<CampusGroupModel> editCampusGroup(Guid id, CreateCampusGroupModel createCampusGroupModel, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);

            if (!account.isAdmin) throw new ForbiddenException("У вас нет прав администратора");

            var group = await _dbContext.Groups.FirstOrDefaultAsync(gr => gr.Id == id);

            if (group == null) throw new NotFoundException("Группы не существует");

            group.Name = createCampusGroupModel.name;

            _dbContext.Groups.Update(group);
            await _dbContext.SaveChangesAsync();

            CampusGroupModel campusGroupModel = new CampusGroupModel()
            {
                name = group.Name,
                id = group.Id
            };
            return campusGroupModel;
        }

        public async Task deleteGroup(Guid groupId, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);
            var group = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null) throw new NotFoundException("Такой группы не существует");

            if (!account.isAdmin) throw new ForbiddenException("У вас нет прав администратора");

            _dbContext.Groups.Remove(group);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CampusCoursePreviewModel>> getCampusGroups(Guid id, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);
            var group = await _dbContext.Groups.Include(group => group.Courses).FirstOrDefaultAsync(gr => gr.Id == id);

            if (group == null) throw new NotFoundException("Такой группы не существует");

            var courses = group.Courses.OrderBy(c => c.Name).Select(course => new CampusCoursePreviewModel()
            {
                id = group.Id,
                name = group.Name,
                semester = course.Semester,
                startYear = course.StartYear,
                maximumStudentsCount = course.MaximumStudentsCount,
                remainingSlotsCount = course.RemainingSlotsCount,
                status = course.Status
            }).ToList();

            return courses;
        }
    }
}
