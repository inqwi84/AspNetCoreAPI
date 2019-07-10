using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeTrackingSystem.Data.Access.Context;
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
            _logger.LogCritical("Getting a the existing records");
            return await _context.Employees.Include(em => em.Department).Select(em=>new EmployeeInfo(){EmployeeFullName = $"{em.LastName} {em.FirstName}", DepartmentName = em.Department.DepartmentName}).ToArrayAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<DepartmentInfo>> GetAllDepartments()
        {
            _logger.LogCritical("Getting a the existing records");
            return await _context.Departments.Select(d=> new DepartmentInfo {DepartmentName = d.DepartmentName,EmployeeCount = d.Employees.Count}).ToArrayAsync().ConfigureAwait(false);
        }
    }
}
