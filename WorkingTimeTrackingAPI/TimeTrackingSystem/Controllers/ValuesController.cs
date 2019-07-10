using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTrackingSystem.Data.Access.DAL;
using TimeTrackingSystem.Data.Model;

namespace TimeTrackingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ITimeTrackingSystemRepository _repository;

        public ValuesController(ITimeTrackingSystemRepository repository)
        {
            _repository = repository;
        }
        // GET api/values
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
