using CampusCourses.Data;
using CampusCourses.Data.DTO.Account;
using CampusCourses.Services.IServices;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using CampusCourses.Services.Exceptions;
using System.Text.RegularExpressions;
using CampusCourses.Data.Entities;
using System.Security.Principal;

namespace CampusCourses.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDBContext _dbContext;
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;
        private readonly TokenBlacklistService _tokenBlacklistService;
        private readonly HelperService _helperService;

        public AccountService(AppDBContext dbContext, PasswordService passwordService, TokenService tokenService, 
            TokenBlacklistService tokenBlacklistService, HelperService helperService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _tokenBlacklistService = tokenBlacklistService;
            _helperService = helperService;
        }

        public async Task<string> registerUser(UserRegisterModel userRegisterModel)
        {
            var repeatEmail = await _dbContext.Accounts.FirstOrDefaultAsync(account => account.Email == userRegisterModel.email);
            const string emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            const string passwordRegex = @"^(?=.*\d).{6,}$";

            if (repeatEmail != null) throw new BadRequestException("Данный email уже используется");

            if (userRegisterModel.password != userRegisterModel.confirmPassword) throw new BadRequestException("Пароли должны быть одинаковыми");

            if (string.IsNullOrWhiteSpace(userRegisterModel.email) || !Regex.IsMatch(userRegisterModel.email, emailRegex))
            {
                throw new BadRequestException("Некорректный email");
            }

            var birthDate = userRegisterModel.birthDate.Date;
            var today = DateTime.Today;

            if (birthDate <= today.AddYears(-99) || birthDate >= today.AddYears(-14))
            {
                throw new BadRequestException("Возраст пользователя должен быть от 14 до 99 лет");
            }

            if (string.IsNullOrEmpty(userRegisterModel.password) || !Regex.IsMatch(userRegisterModel.password, passwordRegex))
            {
                throw new BadRequestException("Пароль должен быть не менее 6 символов и содержать хотя бы одну цифру");
            }

            var newAccount = new Account()
            {
                FullName = userRegisterModel.fullName,
                Password = await _passwordService.HashPassword(userRegisterModel.password),
                BirthDate = userRegisterModel.birthDate.ToUniversalTime(),
                Email = userRegisterModel.email,
                isAdmin = false,
                isStudent = false,
                isTeacher = false,
                CreatedDate = DateTime.UtcNow
            };
            _dbContext.Add(newAccount);
            await _dbContext.SaveChangesAsync();
            string token = _tokenService.GenerateToken(newAccount.Id);
            return token;
        }

        public async Task<string> autorizeUser(UserLoginModel userLoginModel)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Email == userLoginModel.email);

            if (account == null) throw new BadRequestException("Неправильный Email или пароль");

            if (!await _passwordService.VerifyHashedPassword(account.Password, userLoginModel.password)) throw new BadRequestException("Неправильный Email или пароль");

            string token = _tokenService.GenerateToken(account.Id);
            return token;
        }

        public async Task userLogout(Guid userId, string token)
        {
            var account = await _helperService.checkAutorize(userId);

            TimeSpan expiration = TimeSpan.FromDays(1);

            await _tokenBlacklistService.RevokeToken(token, expiration);
        }

        public async Task<UserProfileModel> getUserProfile(Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);

            UserProfileModel userProfileModel = new UserProfileModel()
            { 
                fullName = account.FullName,
                email = account.Email,
                birthDate = account.BirthDate
            };
            return userProfileModel;
        }

        public async Task<UserProfileModel> editProfile(Guid userId, EditUserProfileModel editUserProfileModel)
        {
            var account = await _helperService.checkAutorize(userId);

            var birthDate = editUserProfileModel.birthDate.Date;
            var today = DateTime.Today;

            if (birthDate <= today.AddYears(-99) || birthDate >= today.AddYears(-14))
            {
                throw new BadRequestException("Возраст пользователя должен быть от 14 до 99 лет");
            }

            account.BirthDate = editUserProfileModel.birthDate.ToUniversalTime();
            account.FullName = editUserProfileModel.fullName;

            _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();

            UserProfileModel userProfileModel = new UserProfileModel()
            { 
                email = account.Email,
                birthDate = account.BirthDate,
                fullName = account.FullName
            };
            return userProfileModel;
        }
    }
}
