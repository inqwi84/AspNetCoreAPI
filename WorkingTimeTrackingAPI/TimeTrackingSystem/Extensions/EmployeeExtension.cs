using TimeTrackingSystem.Api.Core.Interfaces.Employee;
using TimeTrackingSystem.Models.Employee;

namespace TimeTrackingSystem.Extensions
{
    public static class EmployeeExtension
    {
        public static DisplayEmployee GetDisplayEmployee(this IEmployee employee)
        {
            return new DisplayEmployee{DepartmentName = employee.DepartmentName,FullName = $"{employee.LastName} {employee.FirstName}", Id =employee.EmployeeId};
        }
    }
}
