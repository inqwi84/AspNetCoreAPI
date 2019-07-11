using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingSystem.Api.Core.Models;

namespace TimeTrackingSystem.Api.Core
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentEmployees>> GetAllDepartments();
        Task<long> AddDepartment(DepartmentInfo department);
        Task<long> UpdateDepartment(DepartmentInfo department);
    }
}
