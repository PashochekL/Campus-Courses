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

        public UserService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
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
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == userId);

            if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

            var roles = new UserRolesModel
            {
                isAdmin = account.isAdmin,
                isStudent = account.isStudent,
                isTeacher = account.isTeacher
            };
            return roles;
        }
    }
}
