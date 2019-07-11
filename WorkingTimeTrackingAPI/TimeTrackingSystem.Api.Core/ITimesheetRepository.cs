using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingSystem.Api.Core.Models;

namespace TimeTrackingSystem.Api.Core
{
    public interface ITimesheetRepository
    {
        Task<bool> StartEmployeeTimeSheet(long employeeId);
        Task<bool> StopEmployeeTimeSheet(long employeeId);
        Task<IEnumerable<TimesheetInfo>> GetEmployeesWorkedHours(DateTime dateFrom, DateTime dateTo);
    }
}
