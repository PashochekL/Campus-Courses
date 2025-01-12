using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CampusCourses.Migrations
{
    /// <inheritdoc />
    public partial class AddCoursesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("1c5b862a-1c71-49d0-8cb9-278f7663ccb8"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("28226f8e-4423-485c-be0b-d40c6417cb2d"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("c7af83fc-e2cc-4fb2-9611-9c53bd332a8e"));

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartYear = table.Column<int>(type: "integer", nullable: false),
                    MaximumStudentsCount = table.Column<int>(type: "integer", nullable: false),
                    RemainingSlotsCount = table.Column<int>(type: "integer", nullable: false),
                    Semester = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Requirements = table.Column<string>(type: "text", nullable: false),
                    Annotations = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "BirthDate", "CreatedDate", "Email", "FullName", "Password", "isAdmin", "isStudent", "isTeacher" },
                values: new object[,]
                {
                    { new Guid("82791f7a-911e-477b-82c6-a0e0b6d121cc"), new DateTime(1995, 2, 12, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "sanyaSigmaBoy@mail.ru", "Саша Сигма Бойчик", "AEIYYdHCkI9aiJfFKEIcGQ7JapZwRimTD7v7PIHgv0+b6nMk6yfGkKXriLesIa1WzQ==", true, true, false },
                    { new Guid("dbbeea11-e240-40b0-8d9f-fe5e1f190274"), new DateTime(2000, 10, 20, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "danilaTrampManovich@mail.ru", "Данила Трампович", "APdTw43V+uIgXrexoxwaHr4/M0iE9cBv5nWYodpx9ynJKMbezEi9vkfQCmXzKNnW5A==", true, false, true },
                    { new Guid("f355e7fa-634d-4ac4-bd33-035f3de6b392"), new DateTime(2005, 6, 16, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "kostyaShvebs@mail.ru", "Костя Швепсов", "AD7F2EIuUb/TrQD5qq0Vmz4wwHLSC8hxoWISKu7i6ocuiqNPzI0iLISdAcaEb0MtoQ==", true, false, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_GroupId",
                table: "Courses",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

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

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "BirthDate", "CreatedDate", "Email", "FullName", "Password", "isAdmin", "isStudent", "isTeacher" },
                values: new object[,]
                {
                    { new Guid("1c5b862a-1c71-49d0-8cb9-278f7663ccb8"), new DateTime(2000, 10, 20, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "danilaTrampManovich@mail.ru", "Данила Трампович", "AClEAUiEaVd8/reRBvVugtzrPPe540nOm+cER7YgV70XH+4hW+5RPMHfXY0mJYnDwA==", true, false, true },
                    { new Guid("28226f8e-4423-485c-be0b-d40c6417cb2d"), new DateTime(1995, 2, 12, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "sanyaSigmaBoy@mail.ru", "Саша Сигма Бойчик", "ANm27etmzDnQcrf0H4SgDgW0NK7WnglEe0PqbhkuMzE8kwd04bz9xDkweBoJpqUL/w==", true, true, false },
                    { new Guid("c7af83fc-e2cc-4fb2-9611-9c53bd332a8e"), new DateTime(2005, 6, 16, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "kostyaShvebs@mail.ru", "Костя Швепсов", "AFlZZX3w9HRHWp44QD/9Aw/3bv0N/lJPvtH27CM9GvkbQXkeVMCas5RRrDphD5ycGg==", true, false, false }
                });
        }
    }
}
