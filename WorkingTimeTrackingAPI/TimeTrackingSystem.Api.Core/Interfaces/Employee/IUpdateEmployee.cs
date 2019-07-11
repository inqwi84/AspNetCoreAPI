namespace TimeTrackingSystem.Api.Core.Interfaces.Employee
{
    public interface IUpdateEmployee : ICreateEmployee
    {
        long? EmployeeId { get; set; }
    }
}
