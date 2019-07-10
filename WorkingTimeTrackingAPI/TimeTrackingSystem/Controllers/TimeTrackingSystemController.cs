using System.Collections.Generic;
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
        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return _repository.GetAllEmployees();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

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
