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

            //third element in the report
            //sperate the users into groups based on the assignedToUserId
            var tasksPerUser = _taskRepo.GetAllTasks()
        
             // 2. The Join Method
            .Join(
                _userRepo.GetAllUsers(),               // The Inner Data (Users)
                task => task.AssignedToUserId.ToString(),       // The Outer Key Selector (Task's reference to the user)
                user => user.Id.ToString(),                                // The Inner Key Selector (User's actual ID)
            
            // 3. The Result Selector (Creating the Anonymous Type)
                (task, user) => new 
                { 
                    TaskId = task.Id, 
                    UserFullName = user.FullName 
                }
            )
        
        // 4. Group by the newly attached Full Name
            .GroupBy(joined => joined.UserFullName)
        
        // 5. Convert directly to a Dictionary where Key = Name, Value = Count
            .ToDictionary(
                group => group.Key,         // The key of the dictionary becomes the group's key (FullName)
                group => group.Count()      // The value becomes the total number of items in that specific group
            );

            //fourth element in the report => high priority incomplete tasks
            int highPriorityIncompleteCount = _taskRepo.GetAllTasks().Count(t => t.TaskStatus != Status.Completed && (t.TaskPriority == Priority.High || t.TaskPriority == Priority.Critical));
        
            //fifth elemet in the report => Persentage of incompleted tasks from date to date
            var fromToDateTasks = _taskRepo.GetAllTasks().Where(t => t.CreatedAt <= FromDate && t.DueDate >= ToDate);
            var IncompletedTaskGroup = fromToDateTasks.GroupBy(t => t.TaskStatus);
            int incompletedtasksCount = 0;
            int completedtasksCount = 0;
            foreach (var group in IncompletedTaskGroup)
            {
                if (group.Key == Status.Completed)
                {
                    completedtasksCount += group.Count();
                }
                else if (group.Key == Status.InProgress || group.Key == Status.Pending)
                {
                    incompletedtasksCount += group.Count();
                }
            }

            double presentage = completedtasksCount/(completedtasksCount+incompletedtasksCount) / 100;
            TaskSummaryReportDto report = new(tasksByStatus, numberOfOverDueTasks, tasksPerUser, highPriorityIncompleteCount, presentage);
            return report;
        }
    }
}
