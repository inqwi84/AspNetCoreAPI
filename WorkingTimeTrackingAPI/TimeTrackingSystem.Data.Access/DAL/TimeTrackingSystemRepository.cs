using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeTrackingSystem.Api.Core;
using TimeTrackingSystem.Api.Core.Interfaces.Employee;
using TimeTrackingSystem.Api.Core.Models;
using TimeTrackingSystem.Data.Access.Context;
using TimeTrackingSystem.Data.Access.Models;

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

        public async Task<IEnumerable<IEmployee>> GetAllEmployees()
        {
            IEnumerable<IEmployee> result;
            try
            {
                _logger.LogInformation("Getting employees list");
                result = await _context.Employees.Include(em => em.Department).Select(em => new EmployeeInfo(em.LastName, em.FirstName, em.Department.DepartmentId, em.EmployeeId, em.Department.DepartmentName)).ToArrayAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        public async Task<IEnumerable<DepartmentEmployees>> GetAllDepartments()
        {
            _logger.LogInformation("Get departments list");
            return await _context.Departments.Select(d => new DepartmentEmployees { Name = d.DepartmentName, Count = d.Employees.Count }).ToArrayAsync();
        }

        public async Task<long?> AddEmployee(ICreateEmployee employee)
        {
            _logger.LogInformation("Add employee");
            var data = new Employee()
            {
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                DepartmentId = employee.DepartmentId.Value
            };
            _context.Employees.Add(data);
            await _context.SaveChangesAsync();
            return data.EmployeeId;
        }

        public async Task<long?> UpdateEmployee(IUpdateEmployee employee)
        {
            _logger.LogInformation($"Update employee {employee.EmployeeId}");
            var existing = _context.Employees.Find(employee.EmployeeId);
            if (existing == null) throw new Exception("Employee doesn't exist");
            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.DepartmentId = employee.DepartmentId.Value;
            await _context.SaveChangesAsync();
            return employee.EmployeeId;
        }
        public async Task<long> AddDepartment(DepartmentInfo department)
        {
            if (string.IsNullOrWhiteSpace(department.DepartmentName))
            {
                throw new Exception("Department must have name");
            }
            _logger.LogInformation("Add department");
            var data = new Department() { DepartmentName = department.DepartmentName };
            _context.Departments.Add(data);
            await _context.SaveChangesAsync();
            return data.DepartmentId;
        }

        public async Task<long> UpdateDepartment(DepartmentInfo department)
        {
            _logger.LogInformation($"Update department {department.DepartmentId}");
            if (string.IsNullOrWhiteSpace(department.DepartmentName))
            {
                throw new Exception("Department must have name");
            }
            var existing = _context.Departments.Find(department.DepartmentId);
            if (existing == null) throw new Exception("Department doesn't exist");
            existing.DepartmentName = department.DepartmentName;
            await _context.SaveChangesAsync();
            return department.DepartmentId.Value;
        }

        public async Task<bool> StartEmployeeTimeSheet(long employeeId)
        {
            _logger.LogInformation($"Start timesheet for employee {employeeId}");
            var existing = _context.Employees.Find(employeeId);
            if (existing == null) throw new Exception("Employee doesn't exist");

            var lastAction = await _context.Timesheets.LastOrDefaultAsync(t => t.EmployeeId == employeeId);
            if (lastAction == null)
            {
                //First time at work
                _context.Timesheets.Add(new Timesheet() { EmployeeId = employeeId, FinishTime = null, StartTime = DateTime.Now.ToUniversalTime() });
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }

            if (lastAction.FinishTime.HasValue)
            {
                //Last sheet is completed
                _context.Timesheets.Add(new Timesheet() { EmployeeId = employeeId, FinishTime = null, StartTime = DateTime.Now.ToUniversalTime() });
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            throw new Exception("Can't start new timesheet before precedent is not ended");
        }

        public async Task<bool> StopEmployeeTimeSheet(long employeeId)
        {
            _logger.LogInformation($"End timesheet for employee {employeeId}");
            var existing = _context.Employees.Find(employeeId);
            if (existing == null) throw new Exception("Employee doesn't exist");

            var lastAction = await _context.Timesheets.LastOrDefaultAsync(t => t.EmployeeId == employeeId);
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
            var result = await _context.SaveChangesAsync();
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
            }).ToArrayAsync();
        }
    }
}
