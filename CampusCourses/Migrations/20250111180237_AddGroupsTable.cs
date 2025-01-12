using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CampusCourses.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Groups");

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
        }
    }
}
