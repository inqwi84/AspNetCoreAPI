namespace TimeTrackingSystem.Api.Core
{
    /// <summary>
    /// Интерфейс для работы с данными
    /// </summary>
    public interface ITimeTrackingSystemRepository : IDepartmentRepository, IEmployeeRepository, ITimesheetRepository
    {

    }
}
