using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
    using ToDoList.Interfaces;

    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private IDataStore dataStore;

        public TaskController(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        // GET api/values/5
        [HttpGet("{userId:int}")]
        public IActionResult Get([FromRoute] int userId)
        {
            var payload = this.dataStore.Read(userId);
            if (payload == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(payload);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
