﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeTrackingSystem.Data.Access.Context;

namespace TimeTrackingSystem.Data.Access.Migrations
{
    [DbContext(typeof(TimeTrackingSystemDbContext))]
    [Migration("20190709220352_testMigration")]
    partial class testMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("TimeTrackingSystem.Data.Model.Department", b =>
                {
                    b.Property<long>("DepartmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DepartmentName");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("TimeTrackingSystem.Data.Model.Employee", b =>
                {
                    b.Property<long>("EmployeeId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("DepartmentId");

                    b.Property<string>("FirstName");

                    b.Property<long>("IsDeleted");

                    b.Property<string>("LastName");

                    b.HasKey("EmployeeId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("TimeTrackingSystem.Data.Model.Timesheet", b =>
                {
                    b.Property<long>("TimesheetId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("EmployeeId");

                    b.Property<DateTime>("FinishTime");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("TimesheetId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Timesheets");
                });

            modelBuilder.Entity("TimeTrackingSystem.Data.Model.Employee", b =>
                {
                    b.HasOne("TimeTrackingSystem.Data.Model.Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TimeTrackingSystem.Data.Model.Timesheet", b =>
                {
                    b.HasOne("TimeTrackingSystem.Data.Model.Employee")
                        .WithMany("Timesheets")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
