using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Task_Manager.Models;
using Task_managment_system.Repositries;
using Task_managment_system.Services;
using Task_managment_system.DTO;

namespace Task_managment_system.Controllers
{

    [ApiController]
    [Route("/api/reports")]
    [Authorize(Policy = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportServices _reportServices;

        public ReportsController(ReportServices reportServices)
        {
            _reportServices = reportServices;
        }

        [HttpGet("taks-summary")]
        public IActionResult TasksReport()
        {
            TaskSummaryReportDto report = _reportServices.CreateTasksReport();
            return Ok();
        }
    }
}
