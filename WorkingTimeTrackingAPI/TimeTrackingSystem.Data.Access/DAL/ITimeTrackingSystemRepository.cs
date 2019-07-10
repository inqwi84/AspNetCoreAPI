using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingSystem.Data.Model;

namespace TimeTrackingSystem.Data.Access.DAL
{
    public interface ITimeTrackingSystemRepository
    {
        Task<List<Employee>> GetAllEmployees();

        Task<List<Department>> GetAllDepartments();
    }
}
