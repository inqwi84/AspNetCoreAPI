using TimeTrackingSystem.Api.Core.Interfaces.Department;

namespace TimeTrackingSystem.Api.Core.Interfaces.Employee
{
    public interface ICreateEmployee : ICreateDepartment
    {
        string LastName { get; set; }
        string FirstName { get; set; }
    }
}
