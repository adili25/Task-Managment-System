using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_managment_system.Services;
using Task_managment_system.DTO;
using Task_managment_system.Enums;

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

        [HttpGet("task-summary")]
        public async Task<ActionResult> TasksReport([FromQuery] DateTime FromDate, [FromQuery] DateTime ToDate)
        {
            TaskSummaryReportDto report = await _reportServices.CreateTasksReport(FromDate, ToDate);
            return Ok(new
            {
                message = "Tasks Report Successfully",
                report = report
            });
        }
    }
}
