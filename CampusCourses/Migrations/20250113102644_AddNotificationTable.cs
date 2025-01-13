using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CampusCourses.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    IsImportant = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "BirthDate", "CreatedDate", "Email", "FullName", "Password", "isAdmin", "isStudent", "isTeacher" },
                values: new object[,]
                {
                    { new Guid("10a3c264-29a0-40b2-afe6-5e399a55da94"), new DateTime(2005, 6, 16, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "kostyaShvebs@mail.ru", "Костя Швепсов", "AIgnQZeYe8+d8Rx+uzp5o0kfOR2AnJDpqOzkUygJcKq2U28RNO3udtxaRFzbyqgxQw==", true, false, false },
                    { new Guid("257b5bc3-6989-4c18-942a-32987c9a1af5"), new DateTime(1995, 2, 12, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "sanyaSigmaBoy@mail.ru", "Саша Сигма Бойчик", "AAE58GMnMjfjeQbVPvnuBG12NbRmdttYQwQPoleDSBUcDHc2UCvEESeDZM0Eo33jpg==", true, true, false },
                    { new Guid("45cdf059-bb1e-42d4-8255-24f69e5f8521"), new DateTime(2000, 10, 20, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "danilaTrampManovich@mail.ru", "Данила Трампович", "AGVqi97oEJ+xhsXOReOe45zjwP/PMso1gqu46oUtA6pBsFkuly7YxyjDOPJMEbFXuw==", true, false, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CourseId",
                table: "Notifications",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("10a3c264-29a0-40b2-afe6-5e399a55da94"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("257b5bc3-6989-4c18-942a-32987c9a1af5"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("45cdf059-bb1e-42d4-8255-24f69e5f8521"));

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "BirthDate", "CreatedDate", "Email", "FullName", "Password", "isAdmin", "isStudent", "isTeacher" },
                values: new object[,]
                {
                    { new Guid("1478faf1-0a82-419b-8e2b-576af005daaf"), new DateTime(2005, 6, 16, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "kostyaShvebs@mail.ru", "Костя Швепсов", "AL86+TTypVyJaC3kxIWpwm3K1ZuBQDv3jun06I0WESeet61lFeDDo+MJEvrZio9aJg==", true, false, false },
                    { new Guid("7a0a46d9-e6a2-4371-8ebe-33bb738c234a"), new DateTime(2000, 10, 20, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "danilaTrampManovich@mail.ru", "Данила Трампович", "AK7FhAF9WOeLpPRuE1cCo3P2qTN/h9y9z/Pyr/WNtzTHeQSw5US/U/KzjvafvTzjBg==", true, false, true },
                    { new Guid("fd59e7a7-3b31-4b2b-9128-f01088b129e6"), new DateTime(1995, 2, 12, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "sanyaSigmaBoy@mail.ru", "Саша Сигма Бойчик", "ANmG636PxHOat8Y3M3FpZd8Z0Ao1H0QcR301YnvpJP6dgW2ozxFhQ0rB9xp2gbxvvA==", true, true, false }
                });
        }
    }
}
