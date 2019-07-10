using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeTrackingSystem.Data.Access.Context;
using TimeTrackingSystem.Data.Model;
using TimeTrackingSystem.Models;

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

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployees()
        {
            IEnumerable<EmployeeInfo> result = Enumerable.Empty<EmployeeInfo>();
            try
            {
                _logger.LogCritical("Getting a the existing records");
                result = await _context.Employees.Include(em => em.Department).Select(em => new EmployeeInfo() { EmployeeFullName = $"{em.LastName} {em.FirstName}", DepartmentName = em.Department.DepartmentName }).ToArrayAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        public async Task<IEnumerable<DepartmentInfo>> GetAllDepartments()
        {
            _logger.LogCritical("Getting a the existing records");
            return await _context.Departments.Select(d => new DepartmentInfo { DepartmentName = d.DepartmentName, EmployeeCount = d.Employees.Count }).ToArrayAsync().ConfigureAwait(false);
        }

        public async Task<long> AddEmployee(Employee employee)
        {
            _logger.LogCritical("Getting a the existing records");
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return employee.EmployeeId;
        }

        public async Task<long> UpdateEmployee(Employee employee)
        {
            _logger.LogCritical("Getting a the existing records");
            var existing = _context.Employees.Find(employee.EmployeeId);
            if (existing == null) throw new Exception("Employee doesn't exist");
            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.DepartmentId = employee.DepartmentId;
            await _context.SaveChangesAsync();
            return employee.EmployeeId;
        }
        public async Task<long> AddDepartment(Department department)
        {
            _logger.LogCritical("Getting a the existing records");
            _context.Departments.Add(department);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return department.DepartmentId;
        }

        public async Task<long> UpdateDepartment(Department department)
        {
            _logger.LogCritical("Getting a the existing records");
            var existing = _context.Departments.Find(department.DepartmentId);
            if (existing == null) throw new Exception("Department doesn't exist");
            existing.DepartmentName = department.DepartmentName;
            await _context.SaveChangesAsync();
            return department.DepartmentId;
        }

    }
}
