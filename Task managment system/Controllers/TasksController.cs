using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_managment_system.Repositries;
using Task_Manager.Models;
using Task_managment_system.DTO;
using Task_managment_system.Services;

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
        private (string? currentUserId, bool isAdmin) IsAuthorized()
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            return (currentUserId, isAdmin);
        }

        public TaskController(TaskServices taskServices, ILogger<TaskController> logger)
        {
            _taskServices = taskServices;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<TaskItem>> GetTasks([FromQuery] TaskFilters filters)
        {
            _logger.LogInformation("--> starting getting Tasks");

            //the business rules: if the user was Admin => get all the tasks, if user was a Regualar User => get the tasks assigned to them
            var (currentUserId, isAdmin) = IsAuthorized();

            if (currentUserId == null)
            {
                _logger.LogWarning("<-- the user is not registerd");
                return Unauthorized("---the user is not registed---");
            }

            var listOfTasks = _taskServices.GetFilteredTasks(currentUserId, isAdmin, filters);
            //no need for the nullity check, if no tasks return empty list;
            _logger.LogInformation("--> fetching tasks succussful");

            return Ok(new
            {
                message = "---fetching tasks succussful---",
                tasks = listOfTasks.ToList()
            });
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetTaskById([FromRoute] string id)
        {
            _logger.LogInformation("--> starting getting task with id");
            var task = _taskServices.GetTaskById(id);

            if (task == null)
            {
                _logger.LogWarning("<-- no task with ID {id}", id);
                return NotFound($"---no task with ID {id}---");
            }

            var (currentUserId, isAdmin) = IsAuthorized();

            if (task.CreatedByUserId.ToString() != currentUserId && task.AssignedToUserId.ToString() != currentUserId && !isAdmin)
            {
                _logger.LogWarning("<--the user is not authrized");
                return StatusCode(StatusCodes.Status403Forbidden, "---the user is not authrized---");
            }

            _logger.LogInformation("--> fetching task succussful");
            
            return Ok(new
            {
                message = "---fetching task succussful---",
                task = task
            });
        }
        
        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskDto requestTask)
        {
            _logger.LogInformation("--> starting creating task");

            TaskItem newTask = new (
                    requestTask.Title,
                    requestTask.Discreption,
                    requestTask.Priority,
                    requestTask.DueDate,
                    requestTask.CreatedByUserId,
                    requestTask.AssignedToUserId
                );

            var createdTask = _taskServices.AddTask(newTask);

            _logger.LogInformation("--> creating task succussful");

            return CreatedAtAction(
            nameof(GetTaskById),  // The name of your get method
            new { id = createdTask.Id },  //the route parameter needed for the get method
            createdTask  //the actual object
            );    
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateTask([FromRoute] string id, [FromBody] TaskDto recievedUpdatedTask)
        {
            _logger.LogInformation("--> starting updating task");

            var task = _taskServices.GetTaskById(id);
            if (task == null)
            {
                _logger.LogWarning("<-- the task with given id not found");
                return NotFound("---the task with given id not found---");
            }

            var (currentUserId, isAdmin) = IsAuthorized();

            if (task.CreatedByUserId.ToString() != currentUserId && task.AssignedToUserId.ToString() != currentUserId && !isAdmin)
            {
                _logger.LogWarning("<-- unauthorized user");
                return StatusCode(StatusCodes.Status403Forbidden, "---the user is not authrized---");
            }

            _logger.LogInformation("--> updating the tasks");

            _taskServices.UpdateTask(task, recievedUpdatedTask);

            _logger.LogInformation("--> updating task succussful");

            return Ok(new
            {
                message = "---updating task succussful---",
                task = task
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask([FromRoute] string id)
        {
            _logger.LogInformation("--> starting deleting task");

            if (id == null)
            {
                _logger.LogWarning("<-- the id is null");
                return BadRequest();
            }

            var task = _taskServices.GetTaskById(id);
            if (task == null)
            {
                _logger.LogWarning("<-- the task with given id not found");
                return NotFound("---The Task With Given Id Not Found---");
            }

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");  
            
            if (task.CreatedByUserId.ToString() != currentUserId && task.AssignedToUserId.ToString() != currentUserId && !isAdmin)
            {
                _logger.LogWarning("<-- unauthorized user");
                return StatusCode(StatusCodes.Status403Forbidden, "---the user is not authrized---");
            }

            if (_taskServices.DeleteTask(task))
            {
                _logger.LogInformation("--> deleting task succussful");
                return NoContent();
            }

            else 
            {
                _logger.LogWarning("<-- deleting task failed");
                return NotFound("---The Task With Given Id Not Found---");
            }
        }
    }
}
