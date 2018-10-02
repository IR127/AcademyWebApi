namespace ToDoList.Controllers
{
    using System.Net.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ToDoList.Interfaces;
    using ToDoList.Models;

    [Route("api/[controller]")]
    public class ListController : Controller
    {
        private readonly IDataStore dataStore;

        public ListController(IDataStore dataStore)
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

        // PUT api/values/5
        [HttpPut]
        public IActionResult Put([FromBody] UserTask newTask)
        {
            var route = this.Request.Path.Value;
            return new CreatedResult(route, newTask);
        }

        // PATCH api/values
        [HttpPatch]
        public void Patch([FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
