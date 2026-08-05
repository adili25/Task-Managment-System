using Task_Manager.Models;
using Task_managment_system.DTO;
using Task_managment_system.Repositries;
using Task_managment_system.Enums;

namespace Task_managment_system.Services
{
    public class ReportServices 
    {
        private readonly TaskRepository _taskRepo;

        public ReportServices(TaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }


        public TaskSummaryReportDto CreateTasksReport()
        {
            List<TaskItem> tasks = [.. _taskRepo.GetAllTasks()];

            var StatusGroupedTasks = tasks.GroupBy(t => t.TaskStatus);
            
            //first element in the report => Status: task count
            Dictionary<string, int> tasksByStatus = new Dictionary<string, int>();
            foreach (var group in StatusGroupedTasks)
            {
                tasksByStatus[group.Key.ToString()] = group.Count(); 
            }

            //second element in the report => number of unfinished overdue tasks
            int numberOfOverDueTasks = tasks.Count(t => t.DueDate < DateTime.UtcNow && t.TaskStatus != Status.Completed);

            var UsersGroupedTasks = tasks.GroupBy(t => t.AssignedToUserId);
            
        }
    }
}
