﻿using System.Collections.Generic;

namespace TimeTrackingSystem.Data.Access.Models
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
        public Department Department { get; set; }
        /// <summary>
        /// Флаг уволенного(удаленного) сотрудника
        /// </summary>
        public long IsDeleted { get; set; }
        /// <summary>
        /// Список периодов работы
        /// </summary>
        public ICollection<Timesheet> Timesheets { get; set; }

    }
}
