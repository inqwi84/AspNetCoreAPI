using TimeTrackingSystem.Api.Core.Interfaces.Department;
using TimeTrackingSystem.Api.Core.Interfaces.Employee;

namespace TimeTrackingSystem.Api.Core.Models
{
    public class EmployeeInfo : IEmployee,IDepartment
    {
        public long? EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public long? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public EmployeeInfo(string lastName, string firstName, long departmentId, long emEmployeeId, string departmentName)
        {
            EmployeeId = emEmployeeId;
            LastName = lastName;
            FirstName = firstName;
            DepartmentId = departmentId;
            DepartmentName = departmentName;
        }
    }
}
