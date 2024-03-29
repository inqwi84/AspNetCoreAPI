﻿using System.Collections.Generic;

namespace TimeTrackingSystem.Data.Model
{
    /// <summary>
    /// Модель отдела
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Идентификатор отдела
        /// </summary>
        public long DepartmentId { get; set; }
        /// <summary>
        /// Наименование отдела
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public ICollection<Employee> Employees { get; set; }

    }
}
