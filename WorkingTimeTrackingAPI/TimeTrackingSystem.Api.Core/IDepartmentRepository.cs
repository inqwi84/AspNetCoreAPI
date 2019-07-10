using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingSystem.Data.Model;
using TimeTrackingSystem.Models;

namespace TimeTrackingSystem.Api.Core
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<EmployeeInfo>> GetAllEmployees();
        Task<long> AddDepartment(Department department);
        Task<long> UpdateDepartment(Department department);
    }
}
