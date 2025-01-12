using CampusCourses.Data;
using CampusCourses.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampusCourses.Services
{
    public class StartDataService
    {
        private readonly PasswordService _passwordService;

        public StartDataService(PasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public List<Account> CreateAccounts()
        {
            var accounts = new List<Account>();

            accounts.AddRange(new[]
            {
                new Account // Админ и учитель
                {
                    Id = Guid.NewGuid(),
                    FullName = "Данила Трампович",
                    Password = _passwordService.HashPassword("123123").Result,
                    BirthDate = DateTime.Parse("2000-10-20T16:35:29.390Z").ToUniversalTime(),
                    CreatedDate = DateTime.Parse("2025-01-11T20:56:06.390Z").ToUniversalTime(),
                    Email = "danilaTrampManovich@mail.ru",
                    isTeacher = true,
                    isAdmin = true,
                    isStudent = false
                },
                new Account // Админ
                {
                    Id = Guid.NewGuid(),
                    FullName = "Костя Швепсов",
                    Password = _passwordService.HashPassword("123123").Result,
                    BirthDate = DateTime.Parse("2005-06-16T16:35:29.390Z").ToUniversalTime(),
                    CreatedDate = DateTime.Parse("2025-01-11T20:56:06.390Z").ToUniversalTime(),
                    Email = "kostyaShvebs@mail.ru",
                    isTeacher = false,
                    isAdmin = true,
                    isStudent = false
                },
                new Account // Админ и студент
                {
                    Id = Guid.NewGuid(),
                    FullName = "Саша Сигма Бойчик",
                    Password = _passwordService.HashPassword("123123").Result,
                    BirthDate = DateTime.Parse("1995-02-12T16:35:29.390Z").ToUniversalTime(),
                    CreatedDate = DateTime.Parse("2025-01-11T20:56:06.390Z").ToUniversalTime(),
                    Email = "sanyaSigmaBoy@mail.ru",
                    isTeacher = false,
                    isAdmin = true,
                    isStudent = true
                }
            });
            return accounts;
        }
    }
}
