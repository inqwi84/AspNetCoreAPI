using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingSystem.Api.Core.Interfaces.Employee;

namespace TimeTrackingSystem.Api.Core
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<IEmployee>> GetAllEmployees();
        Task<long?> AddEmployee(ICreateEmployee employee);
        Task<long?> UpdateEmployee(IUpdateEmployee employee);
    }
}
