﻿using Microsoft.EntityFrameworkCore;
using TimeTrackingSystem.Data.Access.Models;

namespace TimeTrackingSystem.Data.Access.Context
{
    public class TimeTrackingSystemDbContext : DbContext
    { 
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public TimeTrackingSystemDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=local.db");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Department>().HasKey(m => m.DepartmentId);
            builder.Entity<Department>().Property(m => m.DepartmentName).IsRequired();
            builder.Entity<Employee>().Property(m => m.EmployeeId).IsRequired();
            builder.Entity<Employee>().Property(m => m.LastName).IsRequired();
            builder.Entity<Employee>().Property(m => m.FirstName).IsRequired();
            builder.Entity<Employee>().HasKey(m => m.EmployeeId);
            builder.Entity<Timesheet>().HasKey(m => m.TimesheetId);
            base.OnModelCreating(builder);
        }
       
    }
}
