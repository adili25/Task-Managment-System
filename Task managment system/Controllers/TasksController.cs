using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_managment_system.Repositries;
using Task_Manager.Models;
using Task_managment_system.DTO;
using Task_managment_system.Services;
using Task_managment_system.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace Task_managment_system.Controllers
{
    [ApiController]
    [Route("/api/task")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly TaskServices _taskServices;
        private readonly ILogger<TaskController> _logger;
        
        //helper method to fetch the UserId from Claims, and isAdmin 
        private (string currentUserId, bool isAdmin) GetUserIdIsAdmin()
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            if (currentUserId == null)
            {
                throw new UnauthorizedException("the user is not authorized");
            }

            return (currentUserId, isAdmin);
        }

        public TaskController(TaskServices taskServices, ILogger<TaskController> logger)
        {
            _taskServices = taskServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskItem>>> GetTasks([FromQuery] TaskFilters filters)
        {
            _logger.LogInformation("--> starting getting Tasks");

            //the business rules: if the user was Admin => get all the tasks, if user was a Regualar User => get the tasks assigned to them
            var (currentUserId, isAdmin) = GetUserIdIsAdmin();

            var listOfTasks = await _taskServices.GetFilteredTasks(currentUserId, isAdmin, filters);
            //no need for the nullity check, if no tasks return empty list;
            _logger.LogInformation("--> fetching tasks succussful");
            var tasks = await listOfTasks.ToListAsync();

            return Ok(new
            {
                message = "---fetching tasks succussful---",
                tasks = tasks
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskById([FromRoute] string id)
        {
            _logger.LogInformation("--> starting getting task with id");
            var (currentUserId, isAdmin) = GetUserIdIsAdmin();

            var task = await _taskServices.GetTaskById(id, currentUserId, isAdmin);

            _logger.LogInformation("--> fetching task succussful");

            return Ok(new
            {
                message = "---fetching task succussful---",
                task = task
            });
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateTask([FromBody] TaskDto requestTask)
        {
            _logger.LogInformation("--> starting creating task");

            var (currentUserId, IsAdmin) = GetUserIdIsAdmin();

            TaskItem newTask = new (
                    requestTask.Title,
                    requestTask.Discreption,
                    requestTask.Priority,
                    requestTask.DueDate,
                    Guid.Parse(currentUserId),
                    requestTask.AssignedToUserId
                );

            var createdTask =await _taskServices.AddTask(newTask);
            _logger.LogInformation("--> creating task succussful");

            return CreatedAtAction(
            nameof(GetTaskById),  // The name of your get method
            new { id = createdTask.Id },  //the route parameter needed for the get method
            createdTask  //the actual object
            );    
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask([FromRoute] string id, [FromBody] UpdateTaskDto recievedUpdatedTask)
        {

            _logger.LogInformation("--> starting updating task");
            var (currentUserId, isAdmin) = GetUserIdIsAdmin();

            var task = await _taskServices.GetTaskById(id, currentUserId, isAdmin);

            _logger.LogInformation("--> updating the tasks");
            await _taskServices.UpdateTask(task, recievedUpdatedTask);

            _logger.LogInformation("--> updating task succussful");

            return Ok(new
            {
                message = "---updating task succussful---",
                task = task
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask([FromRoute] string id)
        {
            _logger.LogInformation("--> starting deleting task");
            var (currentUserId, IsAdmin) = GetUserIdIsAdmin();

            var task = await _taskServices.GetTaskById(id, currentUserId, IsAdmin);

            await _taskServices.DeleteTask(task);
            _logger.LogInformation("--> deleting task succussful");
            return NoContent();
        }
    }
}
