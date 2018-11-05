namespace ToDoList.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Mvc;
    using ToDoList.Interfaces;
    using ToDoList.Models;

    [Route("api/[controller]")]
    public class ListController : Controller
    {
        private readonly IDataStore dataStore;
        private readonly TelemetryClient telemetryClient = new TelemetryClient();

        public ListController(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(new OkResult());
        }

        // GET api/values/5
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId([FromRoute] string userId)
        {
            var payload = await this.dataStore.Read(userId);
            if (!payload.Any())
            {
                this.telemetryClient.TrackEvent($"[{userId}] : NoContent - User has no tasks");
                return new NoContentResult();
            }

            var tasks = payload.Select(basicTask => new AdvanceTask()
                {
                    UserId = basicTask.UserId,
                    TaskId = basicTask.TaskId,
                    Description = basicTask.Description,
                    DueBy = basicTask.DueBy,
                    Completed = basicTask.IsComplete,
                    Added = basicTask.Added,
                    PastDueDate = DateTime.Now > basicTask.DueBy,
                    DueWithin24 = DateTime.Compare(DateTime.Now, basicTask.DueBy) > 0 && DateTime.Compare(DateTime.Now, basicTask.DueBy) < 2
                })
                .ToList();

            tasks = tasks.OrderBy(t => t.Added).ToList();

            this.telemetryClient.TrackEvent($"[{userId}] : Ok - Tasks returned");
            return new OkObjectResult(tasks);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BasicTask task)
        {
            if (task == null)
            {
                this.telemetryClient.TrackEvent($"BadRequest - Invalid task ");
                return new BadRequestObjectResult(new ErrorModel { ErrorCode = "INVALID_REQUEST", ErrorMessage = "Input cannot be null" });
            }

            if (task.Description == null || task.Description.Length < 5)
            {
                this.telemetryClient.TrackEvent($"[{task.UserId}] : BadRequest - Invalid description");
                return new BadRequestObjectResult(new ErrorModel { ErrorCode = "INVALID_REQUEST", ErrorMessage = "Description field cannot be empty/less than 5 characters" });
            }

            task.Added = DateTime.Now;

            var response = await this.dataStore.Create(task);

            if (!response)
            {
                this.telemetryClient.TrackEvent($"[{task.UserId}] : BadRequest - Update Failed");
                return new BadRequestObjectResult(new ErrorModel { ErrorCode = "UPDATE_FAILED", ErrorMessage = "Was not able to add a new task" });
            }

            this.telemetryClient.TrackEvent($"[{task.UserId}] : CreatedResult - New task created");
            var route = this.Request.Path.Value;
            return new CreatedResult(route, task);
        }

        // PATCH api/values
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] BasicTask task)
        {
            if (task == null)
            {
                this.telemetryClient.TrackEvent($"BadRequest - Invalid task");
                return new BadRequestObjectResult(new ErrorModel { ErrorCode = "INVALID_REQUEST", ErrorMessage = "Input cannot be null" });
            }

            if (task.Description == null || task.Description.Length < 5)
            {
                this.telemetryClient.TrackEvent($"[{task.UserId}] : BadRequest - Invalid description");
                return new BadRequestObjectResult(new ErrorModel { ErrorCode = "INVALID_REQUEST", ErrorMessage = "Description field cannot be less than 5 characters" });
            }

            var response = await this.dataStore.Update(task);

            if (!response)
            {
                this.telemetryClient.TrackEvent($"[{task.UserId}] : NotFound - Cannot find user");
                return this.NotFound();
            }

            this.telemetryClient.TrackEvent($"[{task.UserId}] : Ok - Task updated");
            return this.Ok();
        }
    }
}