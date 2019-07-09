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
    }
}
