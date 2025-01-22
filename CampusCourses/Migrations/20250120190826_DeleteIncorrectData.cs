using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CampusCourses.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIncorrectData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("4ca284de-8734-4833-b65d-7454236a0130"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("5f7092e7-d14d-46d3-b32e-26426bd2a7d0"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("ccb86753-c68a-4d6b-8d54-5ba88bb11c32"));

            migrationBuilder.DropColumn(
                name: "isStudent",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "isTeacher",
                table: "Accounts");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "BirthDate", "CreatedDate", "Email", "FullName", "Password", "isAdmin" },
                values: new object[,]
                {
                    { new Guid("26f2a49f-0024-4bd9-805b-28d085f116e7"), new DateTime(2000, 10, 20, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "danilaTrampManovich@mail.ru", "Данила Трампович", "AMsBbohmGnzUrVTiFImzg0lxTzS5fwhrVsPcTRp3q6Fn0OoFrKK7Ne8ezBBjYpuL3Q==", true },
                    { new Guid("64e6ed8c-f5f1-4325-83ad-436b8aaaa34f"), new DateTime(1995, 2, 12, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "sanyaSigmaBoy@mail.ru", "Саша Сигма Бойчик", "AF0GpnJx3PPozSris/1IKMtqwYxOHsr3YkudkWuNWMwHBeuuh+2ZgWKex99mLmCFtQ==", true },
                    { new Guid("bf6377f0-5c0d-43c0-8207-fd9aee9fcf7d"), new DateTime(2005, 6, 16, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "kostyaShvebs@mail.ru", "Костя Швепсов", "ACe+KDwUUoPNTQKJMMzKZDr0VFS43q+4OMAwXdN+OG9q7D2ywgiq5iJ4OwUXkGCb0g==", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("26f2a49f-0024-4bd9-805b-28d085f116e7"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("64e6ed8c-f5f1-4325-83ad-436b8aaaa34f"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("bf6377f0-5c0d-43c0-8207-fd9aee9fcf7d"));

            migrationBuilder.AddColumn<bool>(
                name: "isStudent",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTeacher",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "BirthDate", "CreatedDate", "Email", "FullName", "Password", "isAdmin", "isStudent", "isTeacher" },
                values: new object[,]
                {
                    { new Guid("4ca284de-8734-4833-b65d-7454236a0130"), new DateTime(1995, 2, 12, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "sanyaSigmaBoy@mail.ru", "Саша Сигма Бойчик", "AF6GtHkfBi7UhDUi19VFHKIqXZwtWFP4J4eCvAa+K7fR6PLdEum+z3xaXEnMmnxkNA==", true, true, false },
                    { new Guid("5f7092e7-d14d-46d3-b32e-26426bd2a7d0"), new DateTime(2005, 6, 16, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "kostyaShvebs@mail.ru", "Костя Швепсов", "AAX+Yj6Ag2obyHpZ17JRt/TyP8SuuXZmbIYaZ0SoOL7iWN/CAtF56mYTqbaXcRdbCA==", true, false, false },
                    { new Guid("ccb86753-c68a-4d6b-8d54-5ba88bb11c32"), new DateTime(2000, 10, 20, 16, 35, 29, 390, DateTimeKind.Utc), new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc), "danilaTrampManovich@mail.ru", "Данила Трампович", "AEm6BWx/Zj5FFdABmpCYZ4QiQn7C6eFva6N1caw6GdeoILrfRuGeDw4R/6WXJc+C7w==", true, false, true }
                });
        }
    }
}
