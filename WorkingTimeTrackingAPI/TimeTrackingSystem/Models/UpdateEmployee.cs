using TimeTrackingSystem.Api.Core.Interfaces.Employee;

namespace TimeTrackingSystem.Models
{
    public class UpdateEmployee : IUpdateEmployee
    {
        public long? EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public long? DepartmentId { get; set; }
    }
}
