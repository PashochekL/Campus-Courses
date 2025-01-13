using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CampusCourses.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("82791f7a-911e-477b-82c6-a0e0b6d121cc"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("dbbeea11-e240-40b0-8d9f-fe5e1f190274"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("f355e7fa-634d-4ac4-bd33-035f3de6b392"));

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    MidtermResult = table.Column<int>(type: "integer", nullable: false),
                    FinalResult = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Students_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    mainTeacher = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Teachers_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_Courses_CourseId",
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
                    { new Guid("3450f58e-75b8-4996-a545-f44fe2f657c8"), new DateTime(2005, 6, 16, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "kostyaShvebs@mail.ru", "Костя Швепсов", "AOaacj9/KriUB4OBf8wCm9uzUfEQKzGKy0VY7KyowC2UN11WIeErb8o6+BfXOtH29w==", true, false, false },
                    { new Guid("38525892-8e28-4215-84f2-11c4735872ee"), new DateTime(2000, 10, 20, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "danilaTrampManovich@mail.ru", "Данила Трампович", "AM52gYNd68Vu45htiO3EZt3LpoVeTQWHME2YWw0iTahtAK1GW21xYvaqpyrOWGAnMg==", true, false, true },
                    { new Guid("5262b1b6-b742-4131-8315-878c98e85463"), new DateTime(1995, 2, 12, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "sanyaSigmaBoy@mail.ru", "Саша Сигма Бойчик", "AIFanxb4tJgidseuVB512YL8jRVN4lJkr0bTIdL8rUNtFRLFPmZCHdUli2mYbeiX8Q==", true, true, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_AccountId",
                table: "Students",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CourseId",
                table: "Students",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_AccountId",
                table: "Teachers",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_CourseId",
                table: "Teachers",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teachers");

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

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "BirthDate", "CreatedDate", "Email", "FullName", "Password", "isAdmin", "isStudent", "isTeacher" },
                values: new object[,]
                {
                    { new Guid("82791f7a-911e-477b-82c6-a0e0b6d121cc"), new DateTime(1995, 2, 12, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "sanyaSigmaBoy@mail.ru", "Саша Сигма Бойчик", "AEIYYdHCkI9aiJfFKEIcGQ7JapZwRimTD7v7PIHgv0+b6nMk6yfGkKXriLesIa1WzQ==", true, true, false },
                    { new Guid("dbbeea11-e240-40b0-8d9f-fe5e1f190274"), new DateTime(2000, 10, 20, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "danilaTrampManovich@mail.ru", "Данила Трампович", "APdTw43V+uIgXrexoxwaHr4/M0iE9cBv5nWYodpx9ynJKMbezEi9vkfQCmXzKNnW5A==", true, false, true },
                    { new Guid("f355e7fa-634d-4ac4-bd33-035f3de6b392"), new DateTime(2005, 6, 16, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "kostyaShvebs@mail.ru", "Костя Швепсов", "AD7F2EIuUb/TrQD5qq0Vmz4wwHLSC8hxoWISKu7i6ocuiqNPzI0iLISdAcaEb0MtoQ==", true, false, false }
                });
        }
    }
}
