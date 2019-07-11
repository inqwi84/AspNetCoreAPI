using TimeTrackingSystem.Api.Core.Interfaces.Department;

namespace TimeTrackingSystem.Api.Core.Interfaces.Employee
{
    public interface IEmployee : IUpdateEmployee, IDepartment
    {
    }
}
