using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Task_Manager.Models;
using Task_managment_system.Repositries;

namespace Task_managment_system.Controllers
{

    [ApiController]
    [Route("/api/reports")]
    [Authorize(Policy = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly TaskRepository _taskRepo;
        private readonly UserRepository _userRepo;

        public ReportsController(TaskRepository taskRepo, UserRepository userRepo)
        {
            _taskRepo = taskRepo;
            _userRepo = userRepo;
        }

        [HttpGet("taks-summary")]
        public IActionResult TasksReport()
        {
            List<TaskItem> tasks = [.. _taskRepo.GetAllTasks()];

            var groupedTasks = tasks.GroupBy(t => t.TaskStatus);


            return Ok();
        }
    }
}
