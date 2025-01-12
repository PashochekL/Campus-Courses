using CampusCourses.Data.DTO.Account;

namespace CampusCourses.Services.IServices
{
    public interface IAccountService
    {
        public Task<string> registerUser(UserRegisterModel userRegisterModel);
        public Task<string> autorizeUser(UserLoginModel userLoginModel);
        public Task userLogout(Guid userId, string token);
        public Task<UserProfileModel> getUserProfile(Guid userId);
        public Task<UserProfileModel> editProfile(Guid userId, EditUserProfileModel editUserProfileModel);
    }
}
