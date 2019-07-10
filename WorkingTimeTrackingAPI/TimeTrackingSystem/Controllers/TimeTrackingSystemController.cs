using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTrackingSystem.Data.Access.DAL;
using TimeTrackingSystem.Data.Model;

namespace TimeTrackingSystem.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class TimeTrackingSystemController : ControllerBase
    {
        private readonly ITimeTrackingSystemRepository _repository;

        public TimeTrackingSystemController(ITimeTrackingSystemRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of employees</returns>
        [HttpGet("employee")]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _repository.GetAllEmployees());
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
        public async Task<IActionResult> CreateEmployee([Required] string LastName, [Required]string FirstName, [Required] long DepartmentId)
        {
            try
            {
                return Ok(new { EmployeeId = await _repository.AddEmployee(new Employee { DepartmentId = DepartmentId, LastName = LastName, FirstName = FirstName }) });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update employee
        /// </summary>
        [HttpPut("employee")]
        public async Task<IActionResult> UpdateEmployee([Required] long EmployeeId, [Required] string LastName, [Required]string FirstName, [Required] long DepartmentId)
        {
            try
            {
                return Ok(new { EmployeeId = await _repository.UpdateEmployee(new Employee() { EmployeeId = EmployeeId, DepartmentId = DepartmentId, LastName = LastName, FirstName = FirstName }) });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
