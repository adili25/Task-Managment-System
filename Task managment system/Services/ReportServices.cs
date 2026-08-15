using Microsoft.AspNetCore.Mvc;
using Task_Manager.Models;
using Task_managment_system.DTO;
using Task_managment_system.Enums;
using Task_managment_system.Interfaces;
using Task_managment_system.Exceptions;

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

        private Dictionary<string, int> TasksByStauts(IQueryable<TaskItem> tasks)
        {
            var StatusGroupedTasks = tasks.GroupBy(t => t.TaskStatus);

            //first element in the report => Dictionary<Status, taskCount>
            Dictionary<string, int> tasksByStatus = new Dictionary<string, int>();
            foreach (var group in StatusGroupedTasks)
            {
                tasksByStatus[group.Key.ToString()] = group.Count();
            }

            /*
    --> this is alternative solution to handel all the group and select in the sql server

    Dictionary<string, int> tasksByStatus = _taskRepo.GetAllTasks()
    .GroupBy(t => t.TaskStatus)
    .Select(g => new { 
        Status = g.Key, 
        TaskCount = g.Count() 
    })
    .ToDictionary(
        result => result.Status.ToString(), 
        result => result.TaskCount
    );  
            */

            return tasksByStatus;
        }

        private int NumOfUncompletedTasks(IQueryable<TaskItem> tasks)
        {
            return tasks.Count(t => t.DueDate < DateTime.UtcNow && t.TaskStatus != Status.Completed);
        }

        private Dictionary<string, int> GetTaskCountPerUser(IQueryable<TaskItem> tasks, IQueryable<ApplicationUser> users)
        {
            return tasks
                .Join(
                users,
                task => task.AssignedToUserId.ToString(),
                user => user.Id.ToString(),

                (task, user) => new
                {
                    TaskId = task.Id,
                    UserFullName = user.FullName
                })
                .GroupBy(joined => joined.UserFullName)
                .ToDictionary(
                group => group.Key,
                group => group.Count()
                );
        }

        private int GetHighPriorityUncompletedTasks(IQueryable<TaskItem> tasks)
        {
            return tasks.Count(t => t.TaskStatus != Status.Completed && (t.TaskPriority == Priority.High || t.TaskPriority == Priority.Critical));
        }

        private double PercentageIncompletedTasks(DateTime FromDate, DateTime ToDate, IQueryable<TaskItem> tasks)
        {
            var taskInRange = tasks.Where(t => t.CreatedAt >= FromDate && t.DueDate <= ToDate).ToList();
            double persentage = 0;
            int totalTasks = taskInRange.Count();

            if (totalTasks > 0)
            {
                int completedTasks = taskInRange.Count(t => t.TaskStatus == Status.Completed);
                persentage = (double)completedTasks / totalTasks * 100;
            }

            return persentage;
        }

        public async Task<TaskSummaryReportDto> CreateTasksReport(DateTime FromDate, DateTime ToDate)
        {
            if (FromDate < ToDate)
            {
                throw new ValidationException("The FromDate is after ToDate");
            }

            IQueryable<TaskItem> tasks =  await _taskRepo.GetAllTasks();
            IQueryable<ApplicationUser> users = await _userRepo.GetAllUsers();

            //first element => group tasks by status
            var tasksByStatus = TasksByStauts(tasks);

            //second element in the report => number of unfinished overdue tasks
            var numberOfOverDueTasks = NumOfUncompletedTasks(tasks);

            //third element in the report => sperate the users into groups based on the assignedToUserId
            var tasksPerUser = GetTaskCountPerUser(tasks, users);

            //fourth element in the report => high priority incomplete tasks
            int highPriorityIncompleteCount = GetHighPriorityUncompletedTasks(tasks);

            //fifth elemet in the report => Persentage of incompleted tasks from date to date
            var percentage = PercentageIncompletedTasks(FromDate, ToDate, tasks);


            TaskSummaryReportDto report = new(tasksByStatus, numberOfOverDueTasks, tasksPerUser, highPriorityIncompleteCount, Math.Round(percentage, 2));
            return report;
        }
    }
}
