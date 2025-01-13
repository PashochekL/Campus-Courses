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
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(_dataService.CreateAccounts());

            modelBuilder.Entity<Course>()
                .HasIndex(course => course.Id)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasMany(account => account.MyCourses)
                .WithOne(student => student.Account)
                .HasForeignKey(student => student.UserId);

            modelBuilder.Entity<Account>()
                .HasMany(account => account.TeachingCourses)
                .WithOne(teacher => teacher.Account)
                .HasForeignKey(teacher => teacher.UserId);

            modelBuilder.Entity<Teacher>()
                .HasKey(teacher => new { teacher.UserId, teacher.CourseId });

            modelBuilder.Entity<Teacher>()
                .HasOne(teacher => teacher.Account)
                .WithMany(account => account.TeachingCourses)
                .HasForeignKey(teacher => teacher.UserId);

            modelBuilder.Entity<Teacher>()
                .HasOne(teacher => teacher.Course)
                .WithMany(course => course.Teachers)
                .HasForeignKey(teacher => teacher.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasKey(student => new { student.UserId, student.CourseId });

            modelBuilder.Entity<Student>()
                .HasOne(student => student.Account)
                .WithMany(account => account.MyCourses)
                .HasForeignKey(student => student.UserId);

            modelBuilder.Entity<Student>()
                .HasOne(student => student.Course)
                .WithMany(course => course.Students)
                .HasForeignKey(student => student.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasOne(course => course.Group)
                .WithMany(group => group.Courses)
                .HasForeignKey(course => course.GroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(notification => notification.Course)
                .WithMany(course => course.Notifications)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
