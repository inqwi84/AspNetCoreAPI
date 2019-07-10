using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingSystem.Data.Model;
using TimeTrackingSystem.Models;

namespace TimeTrackingSystem.Data.Access.DAL
{
    public interface ITimeTrackingSystemRepository
    {
        Task<List<Employee>> GetAllEmployees();

        Task<List<DepartmentInfo>> GetAllDepartments();
    }
}
