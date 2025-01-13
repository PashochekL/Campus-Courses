using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CampusCourses.Migrations
{
    /// <inheritdoc />
    public partial class AddNewConnectionsAndKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Accounts_AccountId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Accounts_AccountId",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_AccountId",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_AccountId",
                table: "Students");

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("3450f58e-75b8-4996-a545-f44fe2f657c8"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("38525892-8e28-4215-84f2-11c4735872ee"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("5262b1b6-b742-4131-8315-878c98e85463"));

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Students");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers",
                columns: new[] { "UserId", "CourseId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                columns: new[] { "UserId", "CourseId" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "BirthDate", "CreatedDate", "Email", "FullName", "Password", "isAdmin", "isStudent", "isTeacher" },
                values: new object[,]
                {
                    { new Guid("1478faf1-0a82-419b-8e2b-576af005daaf"), new DateTime(2005, 6, 16, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "kostyaShvebs@mail.ru", "Костя Швепсов", "AL86+TTypVyJaC3kxIWpwm3K1ZuBQDv3jun06I0WESeet61lFeDDo+MJEvrZio9aJg==", true, false, false },
                    { new Guid("7a0a46d9-e6a2-4371-8ebe-33bb738c234a"), new DateTime(2000, 10, 20, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "danilaTrampManovich@mail.ru", "Данила Трампович", "AK7FhAF9WOeLpPRuE1cCo3P2qTN/h9y9z/Pyr/WNtzTHeQSw5US/U/KzjvafvTzjBg==", true, false, true },
                    { new Guid("fd59e7a7-3b31-4b2b-9128-f01088b129e6"), new DateTime(1995, 2, 12, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "sanyaSigmaBoy@mail.ru", "Саша Сигма Бойчик", "ANmG636PxHOat8Y3M3FpZd8Z0Ao1H0QcR301YnvpJP6dgW2ozxFhQ0rB9xp2gbxvvA==", true, true, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Id",
                table: "Courses",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Accounts_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Accounts_UserId",
                table: "Teachers",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Accounts_UserId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Accounts_UserId",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Courses_Id",
                table: "Courses");

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("1478faf1-0a82-419b-8e2b-576af005daaf"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("7a0a46d9-e6a2-4371-8ebe-33bb738c234a"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("fd59e7a7-3b31-4b2b-9128-f01088b129e6"));

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Teachers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Students",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "UserId");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "BirthDate", "CreatedDate", "Email", "FullName", "Password", "isAdmin", "isStudent", "isTeacher" },
                values: new object[,]
                {
                    { new Guid("3450f58e-75b8-4996-a545-f44fe2f657c8"), new DateTime(2005, 6, 16, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "kostyaShvebs@mail.ru", "Костя Швепсов", "AOaacj9/KriUB4OBf8wCm9uzUfEQKzGKy0VY7KyowC2UN11WIeErb8o6+BfXOtH29w==", true, false, false },
                    { new Guid("38525892-8e28-4215-84f2-11c4735872ee"), new DateTime(2000, 10, 20, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "danilaTrampManovich@mail.ru", "Данила Трампович", "AM52gYNd68Vu45htiO3EZt3LpoVeTQWHME2YWw0iTahtAK1GW21xYvaqpyrOWGAnMg==", true, false, true },
                    { new Guid("5262b1b6-b742-4131-8315-878c98e85463"), new DateTime(1995, 2, 12, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "sanyaSigmaBoy@mail.ru", "Саша Сигма Бойчик", "AIFanxb4tJgidseuVB512YL8jRVN4lJkr0bTIdL8rUNtFRLFPmZCHdUli2mYbeiX8Q==", true, true, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_AccountId",
                table: "Teachers",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_AccountId",
                table: "Students",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Accounts_AccountId",
                table: "Students",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Accounts_AccountId",
                table: "Teachers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
