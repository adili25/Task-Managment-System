using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_managment_system.Services;
using Task_managment_system.DTO;

namespace Task_managment_system.Controllers
{

    [ApiController]
    [Route("/api/reports")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportServices _reportServices;

        public ReportsController(ReportServices reportServices)
        {
            _reportServices = reportServices;
        }

        /*
        The Fix: For GET requests filtering by date, you should pass the parameters in the URL query string. Change both attributes to [FromQuery]. This means a frontend application will call your API like this: /api/reports/task-summary?FromDate=2026-08-01&ToDate=2026-08-31
        */
        [HttpGet("task-summary")]
        public IActionResult TasksReport([FromQuery] DateTime FromDate, [FromQuery] DateTime ToDate)
        {
            TaskSummaryReportDto report = _reportServices.CreateTasksReport(FromDate, ToDate);
            return Ok(report);
        }
    }
}
