using CampusCourses.Data.DTO.Account;
using CampusCourses.Data.Entities;
using CampusCourses.Services;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace CampusCourses.Data
{
    public class AppDBContext : DbContext
    {
        private readonly PasswordService _passwordService;
        private readonly StartDataService _dataService;

        public AppDBContext(DbContextOptions<AppDBContext> options, StartDataService dataService) : base(options)
        {
            _dataService = dataService;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(_dataService.CreateAccounts());
        }
    }
}
