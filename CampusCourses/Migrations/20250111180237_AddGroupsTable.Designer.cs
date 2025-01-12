﻿// <auto-generated />
using System;
using CampusCourses.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CampusCourses.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20250111180237_AddGroupsTable")]
    partial class AddGroupsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CampusCourses.Data.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isAdmin")
                        .HasColumnType("boolean");

                    b.Property<bool>("isStudent")
                        .HasColumnType("boolean");

                    b.Property<bool>("isTeacher")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("1c5b862a-1c71-49d0-8cb9-278f7663ccb8"),
                            BirthDate = new DateTime(2000, 10, 20, 16, 35, 29, 390, DateTimeKind.Utc),
                            CreatedDate = new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc),
                            Email = "danilaTrampManovich@mail.ru",
                            FullName = "Данила Трампович",
                            Password = "AClEAUiEaVd8/reRBvVugtzrPPe540nOm+cER7YgV70XH+4hW+5RPMHfXY0mJYnDwA==",
                            isAdmin = true,
                            isStudent = false,
                            isTeacher = true
                        },
                        new
                        {
                            Id = new Guid("c7af83fc-e2cc-4fb2-9611-9c53bd332a8e"),
                            BirthDate = new DateTime(2005, 6, 16, 16, 35, 29, 390, DateTimeKind.Utc),
                            CreatedDate = new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc),
                            Email = "kostyaShvebs@mail.ru",
                            FullName = "Костя Швепсов",
                            Password = "AFlZZX3w9HRHWp44QD/9Aw/3bv0N/lJPvtH27CM9GvkbQXkeVMCas5RRrDphD5ycGg==",
                            isAdmin = true,
                            isStudent = false,
                            isTeacher = false
                        },
                        new
                        {
                            Id = new Guid("28226f8e-4423-485c-be0b-d40c6417cb2d"),
                            BirthDate = new DateTime(1995, 2, 12, 16, 35, 29, 390, DateTimeKind.Utc),
                            CreatedDate = new DateTime(2025, 1, 11, 20, 56, 6, 390, DateTimeKind.Utc),
                            Email = "sanyaSigmaBoy@mail.ru",
                            FullName = "Саша Сигма Бойчик",
                            Password = "ANm27etmzDnQcrf0H4SgDgW0NK7WnglEe0PqbhkuMzE8kwd04bz9xDkweBoJpqUL/w==",
                            isAdmin = true,
                            isStudent = true,
                            isTeacher = false
                        });
                });

            modelBuilder.Entity("CampusCourses.Data.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });
#pragma warning restore 612, 618
        }
    }
}
