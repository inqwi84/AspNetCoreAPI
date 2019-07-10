﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using TimeTrackingSystem.Data.Access.Context;
using TimeTrackingSystem.Data.Model;

namespace TimeTrackingSystem.Data.Access.DAL
{
    public class TimeTrackingSystemRepository : ITimeTrackingSystemRepository
    {
        private readonly TimeTrackingSystemDbContext _context;

        private readonly ILogger _logger;

        public TimeTrackingSystemRepository(TimeTrackingSystemDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("ITimeTrackingSystemRepository");
        }

        public List<Employee> GetAllEmployees()
        {
            _logger.LogCritical("Getting a the existing records");
            return _context.Employees.ToList();
        }

        public List<Department> GetAllDepartments()
        {
            _logger.LogCritical("Getting a the existing records");
            return _context.Departments.ToList();
        }
    }
}