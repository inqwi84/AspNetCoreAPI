using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeTrackingSystem.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с данными
    /// </summary>
    public interface ITimeTrackingSystemRepository
    {
        Task<IEnumerable<EmployeeInfo>> GetAllEmployees();
        Task<IEnumerable<DepartmentInfo>> GetAllDepartments();
        Task<long> AddEmployee(Employee employee);
        Task<long> UpdateEmployee(Employee employee);
        Task<long> AddDepartment(Department department);
        Task<long> UpdateDepartment(Department department);
        Task<bool> StartEmployeeTimeSheet(long employeeId);
        Task<bool> StopEmployeeTimeSheet(long employeeId);
        Task<IEnumerable<TimesheetInfo>> GetEmployeesWorkedHours(DateTime dateFrom, DateTime dateTo);
    }
}
