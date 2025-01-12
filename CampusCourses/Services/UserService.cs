using CampusCourses.Data;
using CampusCourses.Data.DTO.Account;
using CampusCourses.Data.DTO.User;
using CampusCourses.Data.Entities;
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

        /*public async Task<UserShortModel> getListAllUser(Guid userId)
        {
            List<Account> accounts = await _dbContext.Accounts.Include(acc => acc.Id == userId).ToListAsync();

            foreach (var account in accounts)
            {
                UserShortModel userShortModel = new UserShortModel()
                {
                    id = account.Id,
                    fullName = account.FullName
                };
            }
            return userShortModel;
        }*/
    }
}
