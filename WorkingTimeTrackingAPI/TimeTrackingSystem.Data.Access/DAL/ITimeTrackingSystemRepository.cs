using System;
using System.Collections.Generic;
using System.Text;
using TimeTrackingSystem.Data.Model;

namespace TimeTrackingSystem.Data.Access.DAL
{
    public interface ITimeTrackingSystemRepository
    {
        List<Employee> GetAllEmployees();

        List<Department> GetAllDepartments();
    }
}
