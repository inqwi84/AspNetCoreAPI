using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeTrackingSystem.Api.Core;
using TimeTrackingSystem.Data.Access.Context;
using TimeTrackingSystem.Data.Model;
using TimeTrackingSystem.Models;

namespace TimeTrackingSystem.Data.Access.DAL
{
    /// <summary>
    /// Репозиторий данных из SQLite
    /// </summary>
    public class TimeTrackingSystemRepository : ITimeTrackingSystemRepository
    {
        private readonly TimeTrackingSystemDbContext _context;

        private readonly ILogger _logger;

        public TimeTrackingSystemRepository(TimeTrackingSystemDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("TimeTrackingSystemRepository");
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployees()
        {
            IEnumerable<EmployeeInfo> result = Enumerable.Empty<EmployeeInfo>();
            try
            {
                _logger.LogInformation("Getting employees list");
                result = await _context.Employees.Include(em => em.Department).Select(em => new EmployeeInfo { EmployeeFullName = $"{em.LastName} {em.FirstName}", DepartmentName = em.Department.DepartmentName }).ToArrayAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        public async Task<IEnumerable<DepartmentInfo>> GetAllDepartments()
        {
            _logger.LogInformation("Get departments list");
            return await _context.Departments.Select(d => new DepartmentInfo { DepartmentName = d.DepartmentName, EmployeeCount = d.Employees.Count }).ToArrayAsync().ConfigureAwait(false);
        }

        public async Task<long> AddEmployee(Employee employee)
        {
            _logger.LogInformation("Add employee");
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return employee.EmployeeId;
        }

        public async Task<long> UpdateEmployee(Employee employee)
        {
            _logger.LogInformation($"Update employee {employee.EmployeeId}");
            var existing = _context.Employees.Find(employee.EmployeeId);
            if (existing == null) throw new Exception("Employee doesn't exist");
            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.DepartmentId = employee.DepartmentId;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return employee.EmployeeId;
        }
        public async Task<long> AddDepartment(Department department)
        {
            if (string.IsNullOrWhiteSpace(department.DepartmentName))
            {
                throw new Exception("Department must have name");
            }
            _logger.LogInformation("Add department");
            _context.Departments.Add(department);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return department.DepartmentId;
        }

        public async Task<long> UpdateDepartment(Department department)
        {
            _logger.LogInformation($"Update department {department.DepartmentId}");
            if (string.IsNullOrWhiteSpace(department.DepartmentName))
            {
                throw new Exception("Department must have name");
            }
            var existing = _context.Departments.Find(department.DepartmentId);
            if (existing == null) throw new Exception("Department doesn't exist");
            existing.DepartmentName = department.DepartmentName;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return department.DepartmentId;
        }

        public async Task<bool> StartEmployeeTimeSheet(long employeeId)
        {
            _logger.LogInformation($"Start timesheet for employee {employeeId}");
            var existing = _context.Employees.Find(employeeId);
            if (existing == null) throw new Exception("Employee doesn't exist");

            var lastAction = _context.Timesheets.LastOrDefault(t => t.EmployeeId == employeeId);
            if (lastAction == null)
            {
                //First time at work
                _context.Timesheets.Add(new Timesheet() { EmployeeId = employeeId, FinishTime = null, StartTime = DateTime.Now.ToUniversalTime() });
                var result = await _context.SaveChangesAsync().ConfigureAwait(false);
                return result > 0;
            }

            if (lastAction.FinishTime.HasValue)
            {
                //Last sheet is completed
                _context.Timesheets.Add(new Timesheet() { EmployeeId = employeeId, FinishTime = null, StartTime = DateTime.Now.ToUniversalTime() });
                var result = await _context.SaveChangesAsync().ConfigureAwait(false);
                return result > 0;
            }
            throw new Exception("Can't start new timesheet before precedent is not ended");
        }

        public async Task<bool> StopEmployeeTimeSheet(long employeeId)
        {
            _logger.LogInformation($"End timesheet for employee {employeeId}");
            var existing = _context.Employees.Find(employeeId);
            if (existing == null) throw new Exception("Employee doesn't exist");

            var lastAction = _context.Timesheets.LastOrDefault(t => t.EmployeeId == employeeId);
            if (lastAction == null)
            {
                //First time at work
                throw new Exception("Can't end a timesheet before start");
            }

            if (lastAction.FinishTime.HasValue)
            {
                //Last sheet is completed
                throw new Exception("Can't end a timesheet before start");
            }
            lastAction.FinishTime = DateTime.Now.ToUniversalTime();
            var result = await _context.SaveChangesAsync().ConfigureAwait(false);
            return result > 0;
        }

        public async Task<IEnumerable<TimesheetInfo>> GetEmployeesWorkedHours(DateTime dateFrom, DateTime dateTo)
        {
            _logger.LogInformation($"Get worked hours for employees from {dateFrom} to {dateTo}");
            if (dateTo < dateFrom)
            {
                throw new Exception("Start date must be greater than end date");
            }
            return await _context.Employees.Include(em => em.Department).Include(em => em.Timesheets).Select(i => new TimesheetInfo
            {
                EmployeeFullName = $"{i.LastName} {i.FirstName}",
                DepartmentName = i.Department.DepartmentName,
                TotalHours = i.Timesheets != null ? i.Timesheets.Where(t => t.FinishTime != null).Select(t => (t.FinishTime - t.StartTime).Value.TotalHours).Sum() : 0
            }).ToArrayAsync().ConfigureAwait(false);
        }
    }
}
