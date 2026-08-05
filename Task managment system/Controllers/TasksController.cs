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
        private readonly ILogger<TaskController> logger;

        public ActionResult<List<TaskItem>> GetTasks([FromQuery] TaskFilters filters)
        {
            //the business rules here is that if the user was Admin => get all the tasks, if user was a User => get the tasks assigned to them
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            //implement the customizedGetTasks in the TaskServices
            var listOfTasks = _taskServices.GetTasks(currentUserId, isAdmin, filters);
            
            if (listOfTasks == null)
            {
                return BadRequest();
            }

            return Ok(listOfTasks);
        }

        public ActionResult<TaskItem> GetTaskById([FromQuery] string id)
        {
            var task = _taskServices.GetTaskById(id);

            if (task == null)
            {
                return BadRequest($"No Task with ID {id}");
            }

            return Ok(task);
        }


    }
}
