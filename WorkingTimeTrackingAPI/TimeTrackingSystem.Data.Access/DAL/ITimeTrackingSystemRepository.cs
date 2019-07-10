using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingSystem.Data.Model;
using TimeTrackingSystem.Models;

namespace TimeTrackingSystem.Data.Access.DAL
{
    public interface ITimeTrackingSystemRepository
    {
        Task<IEnumerable<EmployeeInfo>> GetAllEmployees();

        Task<IEnumerable<DepartmentInfo>> GetAllDepartments();

        Task<long> AddEmployee(Employee employee);
    }
}
