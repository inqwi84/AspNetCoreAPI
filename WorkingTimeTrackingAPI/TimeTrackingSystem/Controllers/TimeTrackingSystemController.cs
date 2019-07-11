using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeTrackingSystem.Api.Core;
using TimeTrackingSystem.Api.Core.Models;
using TimeTrackingSystem.Extensions;
using TimeTrackingSystem.Models;

namespace TimeTrackingSystem.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class TimeTrackingSystemController : ControllerBase
    {
        private readonly ITimeTrackingSystemRepository _repository;
        private readonly ILogger _logger;
        public TimeTrackingSystemController(ITimeTrackingSystemRepository repository, ILogger<TimeTrackingSystemController> logger)
        {
            _logger = logger;
            _repository = repository;
        }
        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of employees</returns>
        [HttpGet("employee")]
        public async Task<ActionResult> GetEmployees()
        {
            //пример логирования в файл
            _logger.LogInformation("Get employees");
            try
            {
                var employeeResult = await _repository.GetAllEmployees();
                return Ok(employeeResult.Select(t=>t.GetDisplayEmployee()));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// Get all departments
        /// </summary>
        /// <returns>List of departments</returns>
        [HttpGet("department")]
        public async Task<ActionResult> GetDepartments()
        {
            try
            {
                return Ok(await _repository.GetAllDepartments());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Add employee
        /// </summary>
        [HttpPost("employee")]
        public async Task<IActionResult> CreateEmployee([FromQuery]CreateEmployee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.LastName))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Фамилия не может быть пустой");
            }
            if (string.IsNullOrWhiteSpace(employee.FirstName))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Имя не может быть пустым");
            }
            if (employee.DepartmentId.HasValue == false)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Не задан идентификатор отдела");
            }
            try
            {
                return Ok(new { EmployeeId = await _repository.AddEmployee(employee) });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        /// <summary>
        /// Update employee
        /// </summary>
        [HttpPut("employee")]
        public async Task<IActionResult> UpdateEmployee([FromQuery]UpdateEmployee employee)
        {
            if (employee.EmployeeId.HasValue == false)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Не задан идентификатор пользователя");
            }
            if (string.IsNullOrWhiteSpace(employee.LastName))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Фамилия не может быть пустой");
            }
            if (string.IsNullOrWhiteSpace(employee.FirstName))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Имя не может быть пустым");
            }
            if (employee.DepartmentId.HasValue == false)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Не задан идентификатор отдела");
            }
            try
            {
                await _repository.UpdateEmployee(employee);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// Add department
        /// </summary>
        [HttpPost("department")]
        public async Task<IActionResult> CreateDepartment([Required] string Name)
        {
            try
            {
                var id = new { DepartmentId = await _repository.AddDepartment(new DepartmentInfo { DepartmentName = Name }) };
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update department
        /// </summary>
        [HttpPut("department")]
        public async Task<IActionResult> UpdateDepartment([Required] string Name, [Required] long DepartmentId)
        {
            try
            {
                await _repository.UpdateDepartment(new DepartmentInfo() { DepartmentId = DepartmentId, DepartmentName = Name });
                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        ///  Register employee action
        /// </summary>
        [HttpPost("timesheet")]
        public async Task<IActionResult> RegisterEmployeeAction([Required] long EmployeeId, [Required] bool Action)
        {
            try
            {
                return Action == false ? Ok(await _repository.StartEmployeeTimeSheet(EmployeeId)) : Ok(await _repository.StopEmployeeTimeSheet(EmployeeId));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get employees total hours at work
        /// </summary>
        /// <returns>List of employees</returns>
        [HttpGet("timesheet")]
        public async Task<ActionResult> GetEmployeesWorkedHours([Required] DateTime DateFrom, [Required] DateTime DateTo)
        {
            try
            {
                return Ok(new { StartPeriod = DateFrom, EndPeriod = DateTo, Employee = await _repository.GetEmployeesWorkedHours(DateFrom, DateTo) });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
