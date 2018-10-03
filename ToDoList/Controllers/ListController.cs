namespace ToDoList.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                return new NoContentResult();
            }

            var tasks = new List<AdvanceTask>();

            foreach (BasicTask basicTask in payload)
            {
                tasks.Add(new AdvanceTask()
                {
                    UserId = basicTask.UserId,
                    TaskId = basicTask.TaskId,
                    Description = basicTask.Description,
                    DueBy = basicTask.DueBy,
                    Completed = basicTask.Completed,
                    Added = basicTask.Added,
                    PastDueDate = DateTime.Now > basicTask.DueBy,
                    DueWithin24 = DateTime.Compare(DateTime.Now, basicTask.DueBy) > 0 && DateTime.Compare(DateTime.Now, basicTask.DueBy) < 2
                });
            }

            tasks = tasks.OrderBy(t => t.Added).ToList();

            return new OkObjectResult(tasks);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] BasicTask task)
        {

            if (task.Description == null || task.Description.Length < 5)
            {
                return new BadRequestObjectResult("Description field cannot be empty/less than 5 characters");
            }

            task.Added = DateTime.Now;

            var response = this.dataStore.Create(task);

            if (!response)
            {
                return this.StatusCode(500);
            }

            var route = this.Request.Path.Value;
            return new CreatedResult(route, task);

        }

        // PATCH api/values
        [HttpPatch]
        public IActionResult Patch([FromBody] BasicTask task)
        {
            if (task.Description.Length < 5)
            {
                return new BadRequestObjectResult("Description field cannot be empty/less than 5 characters");
            }

            var response = this.dataStore.Update(task);

            if (!response)
            {
                return this.StatusCode(404);
            }

            return this.Ok();
        }
    }
}
