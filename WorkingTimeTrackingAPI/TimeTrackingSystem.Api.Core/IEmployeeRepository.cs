using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingSystem.Data.Model;
using TimeTrackingSystem.Models;

namespace TimeTrackingSystem.Api.Core
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<DepartmentInfo>> GetAllDepartments();
        Task<long> AddEmployee(Employee employee);
        Task<long> UpdateEmployee(Employee employee);
    }
}
