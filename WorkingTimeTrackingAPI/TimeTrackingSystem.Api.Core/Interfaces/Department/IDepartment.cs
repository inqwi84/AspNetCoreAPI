namespace TimeTrackingSystem.Api.Core.Interfaces.Department
{
    public interface IDepartment : ICreateDepartment
    {
        string DepartmentName { get; set; }
    }
}
