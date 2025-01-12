using CampusCourses.Data;
using CampusCourses.Data.DTO.Group;
using CampusCourses.Data.Entities;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace CampusCourses.Services
{
    public class GroupService : IGroupService
    {
        private readonly AppDBContext _dbContext;

        public GroupService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CampusGroupModel>> getListAllGroups(Guid userId)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == userId);

            if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

            var groups = await _dbContext.Groups.OrderBy(gr => gr.Name)
                .Select(group => new CampusGroupModel
                {
                    id = group.Id,
                    name = group.Name
                }).ToListAsync();

            /*if (!groups.Any())
            {
                throw new Exception("Список групп пуст");
            }*/
            return groups;
        }

        public async Task<CampusGroupModel> createGroup(CreateCampusGroupModel createCampusGroupModel, Guid userId)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == userId);

            if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

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
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == userId);

            if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

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
    }
}
