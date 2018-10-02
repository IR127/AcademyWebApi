namespace ToDoList.Controllers
{
    using System;
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

        // POST api/values/5
        [HttpPost]
        public IActionResult Post([FromBody] BasicTask task)
        {
            task.TaskId = Guid.NewGuid();

            if (task.Description == null || task.Description.Length < 5)
            {
                return new BadRequestObjectResult("Description field cannot be empty/less than 5 characters");
            }

            var response = this.dataStore.Create(task);
            if (response)
            {
                var route = this.Request.Path.Value;
                return new CreatedResult(route, task);
            }

            return this.StatusCode(500);
        }

        // PATCH api/values
        [HttpPatch]
        public void Patch([FromBody] string value)
        {
        }
    }
}
