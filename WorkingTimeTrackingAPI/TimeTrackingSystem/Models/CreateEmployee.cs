using TimeTrackingSystem.Api.Core;
using TimeTrackingSystem.Api.Core.Interfaces.Employee;

namespace TimeTrackingSystem.Models
{
    public class CreateEmployee:ICreateEmployee
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public long? DepartmentId { get; set; }
    }
}
