using Task_Manager.Models;
using Task_managment_system.DTO;
using Task_managment_system.Repositries;
using Task_managment_system.Enums;

namespace Task_managment_system.Services
{
    public class ReportServices 
    {
        private readonly TaskRepository _taskRepo;
        private readonly UserRepository _userRepo;

        public ReportServices(TaskRepository taskRepo, UserRepository userRepo)
        {
            _taskRepo = taskRepo;
            _userRepo = userRepo;
        }

        public TaskSummaryReportDto CreateTasksReport(DateTime FromDate, DateTime ToDate)
        {
            IQueryable<TaskItem> tasks =  _taskRepo.GetAllTasks();

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

            //second element in the report => number of unfinished overdue tasks
            int numberOfOverDueTasks = _taskRepo.GetAllTasks().Count(t => t.DueDate < DateTime.UtcNow && t.TaskStatus != Status.Completed);

            //third element in the report => sperate the users into groups based on the assignedToUserId
            var tasksPerUser = _taskRepo.GetAllTasks()
        
            .Join(
                _userRepo.GetAllUsers(),
                task => task.AssignedToUserId.ToString(),
                user => user.Id.ToString(),
            
                (task, user) => new 
                { 
                    TaskId = task.Id, 
                    UserFullName = user.FullName 
                }
            )
        
            .GroupBy(joined => joined.UserFullName)
        
            .ToDictionary(
                group => group.Key,
                group => group.Count()
            );

            //fourth element in the report => high priority incomplete tasks
            int highPriorityIncompleteCount = _taskRepo.GetAllTasks().Count(t => t.TaskStatus != Status.Completed && (t.TaskPriority == Priority.High || t.TaskPriority == Priority.Critical));
        
            //fifth elemet in the report => Persentage of incompleted tasks from date to date
            var taskInRange = _taskRepo.GetAllTasks().Where(t => t.CreatedAt >= FromDate && t.DueDate <= ToDate).ToList();
            double persentage = 0;
            int totalTasks = taskInRange.Count();

            if (totalTasks > 0)
            {
                int completedTasks = taskInRange.Count(t => t.TaskStatus == Status.Completed);
                persentage = (double)completedTasks / totalTasks * 100;
            }
          
            TaskSummaryReportDto report = new(tasksByStatus, numberOfOverDueTasks, tasksPerUser, highPriorityIncompleteCount, Math.Round(persentage, 2));
            return report;
        }
    }
}
