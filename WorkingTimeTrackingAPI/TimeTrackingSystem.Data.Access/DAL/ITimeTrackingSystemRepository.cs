using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingSystem.Data.Model;
using TimeTrackingSystem.Models;

namespace TimeTrackingSystem.Data.Access.DAL
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
