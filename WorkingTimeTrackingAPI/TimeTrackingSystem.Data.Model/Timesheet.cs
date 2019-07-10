using System;

namespace TimeTrackingSystem.Data.Model
{
    /// <summary>
    /// Модель периода рабочего времени сотрудника
    /// </summary>
    public class Timesheet
    {
        /// <summary>
        /// Идентификатор периода времени
        /// </summary>
        public long TimesheetId { get; set; }
        /// <summary>
        /// Идентификатор сотрудника
        /// </summary>
        public long EmployeeId { get; set; }
        /// <summary>
        /// Начало рабочего времени
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Окончание рабочего времени
        /// </summary>
        public DateTime FinishTime { get; set; }

    }
}
