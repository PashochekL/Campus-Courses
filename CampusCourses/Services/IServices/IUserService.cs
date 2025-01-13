using CampusCourses.Data.DTO.User;

namespace CampusCourses.Services.IServices
{
    public interface IUserService
    {
        public Task<List<UserShortModel>> getListAllUser(Guid userId);
        public Task<UserRolesModel> getUserRoles(Guid userId);
    }
}
