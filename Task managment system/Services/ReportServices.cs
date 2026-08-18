using Task_Manager.Models;
using Task_managment_system.DTO;
using Task_managment_system.Enums;
using Task_managment_system.Interfaces;
using Task_managment_system.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Task_managment_system.Services
{
    public class ReportServices 
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IUserRepository _userRepo;

        public ReportServices(ITaskRepository taskRepo, IUserRepository userRepo)
        {
            _taskRepo = taskRepo;
            _userRepo = userRepo;
        }

        private async Task<Dictionary<string, int>> TasksByStauts(IQueryable<TaskItem> tasks)
        {
            Dictionary<string, int> tasksByStatus = await tasks
                .GroupBy(t => t.TaskStatus)
                .Select(g => new { 
                    Status = g.Key,
                    TaskCount = g.Count() 
                })
                .ToDictionaryAsync(
                result => result.Status.ToString(),
                result => result.TaskCount
                );  

            return tasksByStatus;
        }

        private async Task<int> NumOfUncompletedTasks(IQueryable<TaskItem> tasks)
        {
            return await tasks.CountAsync(t => t.DueDate < DateTimeOffset.UtcNow && t.TaskStatus != Status.Completed);
        }

        private async Task<Dictionary<string, int>> GetTaskCountPerUser(IQueryable<TaskItem> tasks, IQueryable<ApplicationUser> users)
        {
            return await tasks
                .Join(
                users,
                task => task.AssignedToUserId,
                user => user.Id,
                (task, user) => new
                {
                    TaskId = task.Id,
                    UserFullName = user.FullName
                })
                .GroupBy(joined => joined.UserFullName)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToDictionaryAsync(
                group => group.Name,
                group => group.Count
                );
        }

        private async Task<int> GetHighPriorityUncompletedTasks(IQueryable<TaskItem> tasks)
        {
            return await tasks.CountAsync(t => t.TaskStatus != Status.Completed && (t.TaskPriority == Priority.High || t.TaskPriority == Priority.Critical));
        }

        private async Task<double> PercentageIncompletedTasks(DateTimeOffset FromDate, DateTimeOffset ToDate, IQueryable<TaskItem> tasks)
        {
            var taskInRange = tasks.Where(t => t.CreatedAt >= FromDate && t.DueDate <= ToDate);
            double persentage = 0;
            int totalTasks = await taskInRange.CountAsync();

            if (totalTasks > 0)
            {
                int completedTasks = await taskInRange.CountAsync(t => t.TaskStatus == Status.Completed);
                persentage = (double)completedTasks / totalTasks * 100;
            }

            return persentage;
        }

        public async Task<TaskSummaryReportDto> CreateTasksReport(DateTimeOffset FromDate, DateTimeOffset ToDate)
        {
            if (FromDate > ToDate)
            {
                throw new ValidationException("The FromDate is after ToDate");
            }

            IQueryable<TaskItem> tasks =  await _taskRepo.GetAllTasks();
            IQueryable<ApplicationUser> users = await _userRepo.GetAllUsers();

            //first element => group tasks by status
            var tasksByStatus = await TasksByStauts(tasks);

            //second element in the report => number of unfinished overdue tasks
            var numberOfOverDueTasks = await NumOfUncompletedTasks(tasks);

            //third element in the report => sperate the users into groups based on the assignedToUserId
            var tasksPerUser = await GetTaskCountPerUser(tasks, users);

            //fourth element in the report => high priority incomplete tasks
            int highPriorityIncompleteCount = await GetHighPriorityUncompletedTasks(tasks);

            //fifth elemet in the report => Persentage of incompleted tasks from date to date
            var percentage = await PercentageIncompletedTasks(FromDate, ToDate, tasks);


            TaskSummaryReportDto report = new(tasksByStatus, numberOfOverDueTasks, tasksPerUser, highPriorityIncompleteCount, Math.Round(percentage, 2));
            return report;
        }
    }
}
