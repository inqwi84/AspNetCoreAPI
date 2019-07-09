using System.Collections.Generic;

namespace TimeTrackingSystem.Data.Model
{
    /// <summary>
    /// Модель сотрудника
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Идентификатор сотрудника
        /// </summary>
        public long EmployeeId { get; set; }
        /// <summary>
        /// Имя сотрудника
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Идентификатор отдела сотрудника
        /// </summary>
        public long DepartmentId { get; set; }
        /// <summary>
        /// Флаг уволенного(удаленного) сотрудника
        /// </summary>
        public long IsDeleted { get; set; }
        /// <summary>
        /// Список периодов работы
        /// </summary>
        public IList<Timesheet> Timesheets { get; set; }

        public Employee()
        {
            Timesheets=new List<Timesheet>();
        }
    }
}
