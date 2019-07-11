using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeTrackingSystem.Api.Core;
using TimeTrackingSystem.Api.Core.Models;
using TimeTrackingSystem.Extensions;
using TimeTrackingSystem.Models;
using TimeTrackingSystem.Models.Employee;

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
                return Ok(employeeResult.Select(t => t.GetDisplayEmployee()));
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
            if (string.IsNullOrWhiteSpace(Name))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Название отдела не может быть пустым");
            }
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
        public async Task<IActionResult> UpdateDepartment([Required] string Name, [Required] long? DepartmentId)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Название отдела не может быть пустым");
            }

            if (DepartmentId.HasValue == false)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Не задан идентификатор отдела");
            }
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
        public async Task<IActionResult> RegisterEmployeeAction([Required] long? EmployeeId, bool? Action)
        {
            if (EmployeeId.HasValue == false)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Не задан сотрудник");
            }
            if (Action.HasValue == false)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Не задано действие");
            }
            try
            {
                return Action == false ? Ok(await _repository.StartEmployeeTimeSheet(EmployeeId.Value)) : Ok(await _repository.StopEmployeeTimeSheet(EmployeeId.Value));
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
        public async Task<ActionResult> GetEmployeesWorkedHours([FromQuery]TimesheetPeriod period)
        {
            if (period.DateFrom.HasValue == false)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Не задана дата начала");
            }
            if (period.DateTo.HasValue == false)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Не задана дата окончания");
            }
            if (period.DateTo < period.DateFrom)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Дата окончания не может быть больше даты начала");
            }
            try
            {
                return Ok(new { StartPeriod = period.DateFrom, EndPeriod = period.DateTo, Employee = await _repository.GetEmployeesWorkedHours(period.DateFrom.Value, period.DateTo.Value) });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
