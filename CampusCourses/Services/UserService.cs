using CampusCourses.Data;
using CampusCourses.Data.DTO.Account;
using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.DTO.User;
using CampusCourses.Data.Entities;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CampusCourses.Services
{
    public class UserService : IUserService
    {
        private readonly AppDBContext _dbContext;
        private readonly HelperService _helperService;

        public UserService(AppDBContext dbContext, HelperService helperService)
        {
            _dbContext = dbContext;
            _helperService = helperService;
        }

        public async Task<List<UserShortModel>> getListAllUser(Guid userId)
        {
            var account = await _dbContext.Accounts.Include(acc => acc.TeachingCourses).FirstOrDefaultAsync(acc => acc.Id == userId);

            if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

            var mainTeacher = account.TeachingCourses.Any(teacher => teacher.mainTeacher);

            if (!(account.isAdmin || mainTeacher)) throw new ForbiddenException("У вас недостаточно прав");

            var users = await _dbContext.Accounts.OrderBy(acc => acc.FullName).Select(acc => new UserShortModel
            { 
                id = acc.Id,
                fullName = acc.FullName
            }).ToListAsync();

            return users;
        }

        public async Task<UserRolesModel> getUserRoles(Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);

            var checkTeacher = await _dbContext.Teachers.AnyAsync(t => t.UserId == userId);
            var checkStudent = await _dbContext.Students.AnyAsync(t => t.UserId == userId);

            var roles = new UserRolesModel
            {
                isAdmin = account.isAdmin,
                isStudent = checkStudent,
                isTeacher = checkTeacher
            };
            return roles;
        }
    }
}
