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
        
        public TaskController(TaskServices taskServices, ILogger<TaskController> logger)
        {
            _taskServices = taskServices;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<TaskItem>> GetTasks([FromBody] TaskFilters filters)
        {
            _logger.LogInformation("starting getting Tasks");
            //the business rules: if the user was Admin => get all the tasks, if user was a Regualar User => get the tasks assigned to them
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            if (currentUserId == null)
            {
                _logger.LogWarning("the user is not registerd");
                return Unauthorized("the user is not registed");
            }

            var listOfTasks = _taskServices.GetFilteredTasks(currentUserId, isAdmin, filters);
            //no need for the nullity check, if no tasks return empty list;
            _logger.LogInformation("fetching tasks succussful");

            return Ok(new
            {
                message = "fetching tasks succussful",
                tasks = listOfTasks.ToList()
            });
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetTaskById([FromRoute] string id)
        {
            _logger.LogInformation("starting getting task with id");
            var task = _taskServices.GetTaskById(id);

            if (task == null)
            {
                _logger.LogWarning("no task with ID {id}", id);
                return NotFound($"no task with ID {id}");
            }

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");            
            if (task.CreatedByUserId.ToString() != currentUserId && !isAdmin)
            {
                _logger.LogWarning("the user is not authrized");
                return Forbid("the user is not authrized");
            }

            _logger.LogInformation("fetching task succussful");
            
            return Ok(new
            {
                message = "fetching task succussful",
                task = task
            });
        }
        
        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskDto requestTask)
        {
            _logger.LogInformation("starting creating task");

            TaskItem newTask = new (
                    requestTask.Title,
                    requestTask.Discreption,
                    requestTask.Priority,
                    requestTask.DueDate,
                    requestTask.CreatedByUserId,
                    requestTask.AssignedToUserId
                );

            var createdTask = _taskServices.AddTask(newTask);

            _logger.LogInformation("creating task succussful");

            return CreatedAtAction(
            nameof(GetTaskById),           // The name of your GET method
            new { id = createdTask.Id },   // The route parameter needed for the GET method
            createdTask                    // The actual object returned in the response body
            );    
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateTask([FromRoute] string id, [FromBody] TaskDto RecivedUpdatedTask)
        {
            _logger.LogInformation("starting updating task");

            if (id == null)
            {
                _logger.LogWarning("the id is null");
                return BadRequest("the id is null");
            }

            var task = _taskServices.GetTaskById(id);
            if (task == null)
            {
                _logger.LogWarning("the task with given id not found");
                return NotFound("the task with given id not found");
            }
    
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");            
            if (task.CreatedByUserId.ToString() != currentUserId && !isAdmin)
            {
                return Forbid("the user is not authrized");
            }

            _logger.LogInformation("updating the tasks");
            _taskServices.UpdateTask(task, RecivedUpdatedTask);

            _logger.LogInformation("updating task succussful");

            return Ok(new
            {
                message = "updating task succussful",
                task = task
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask([FromRoute] string id)
        {
            _logger.LogInformation("starting deleting task");

            if (id == null)
            {
                _logger.LogWarning("the id is null");
                return BadRequest();
            }

            var task = _taskServices.GetTaskById(id);
            if (task == null)
            {
                _logger.LogWarning("the task with given id not found");
                return NotFound("The Task With Given Id Not Found");
            }

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");            
            if (task.CreatedByUserId.ToString() != currentUserId && !isAdmin)
            {
                return Forbid("the user is not authrized");
            }

            if (_taskServices.DeleteTask(task))
            {
                _logger.LogInformation("deleting task succussful");
                return NoContent();
            }

            else 
            {
                _logger.LogWarning("deleting task failed");
                return NotFound("The Task With Given Id Not Found");
            }
        }
    }
}
