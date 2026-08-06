using Task_Manager.Models;
using Task_managment_system.DTO;
using Task_managment_system.Repositries;

namespace Task_managment_system.Services
{
    public class TaskServices
    {
        private readonly TaskRepository _taskRepo;

        public TaskServices(TaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }
        public List<TaskItem> GetTasks(string userId, bool isAdmin, TaskFilters filters)
        {
            List<TaskItem> tasks = _taskRepo.GetAllTasks().ToList();

            if (!isAdmin)
            {
                tasks = [.. tasks.Where(t => t.AssignedToUserId.ToString() == userId)];
                //[.. ] => tasks.Where(...).ToList();
            }

            if (filters.TitleOrDescription != null)
            {
                tasks = [.. tasks.Where(t => t.Title.Contains(filters.TitleOrDescription) || t.Description.Contains(filters.TitleOrDescription))];
            }

            if (filters.Status != null)
            {
                tasks = [.. tasks.Where(t => t.TaskStatus == filters.Status)];
            }

            if (filters.Priority != null)
            {
                tasks = [.. tasks.Where(t => t.TaskPriority == filters.Priority)];
            }

            if (!string.IsNullOrEmpty(filters.AssignedId))
            {
                tasks = [.. tasks.Where(t => t.AssignedToUserId.ToString() == filters.AssignedId)];
            }

            if (!string.IsNullOrEmpty(filters.CreatorId))
            {
                tasks = [.. tasks.Where(t => t.CreatedByUserId.ToString() == filters.CreatorId)];
            }

            if (filters.FromDueDate != null)
            {
                tasks = [.. tasks.Where(t => t.DueDate >= filters.FromDueDate)];
            }

            if (filters.ToDueDate != null)
            {
                tasks = [.. tasks.Where(t => t.DueDate <= filters.ToDueDate)];
            }

            return tasks;
        }

        public TaskItem? GetTaskById(string id)
        {
            var task = _taskRepo.GetTaskById(id);

            if (task != null)
            {
                return task;
            }
            return null;
        }

        public bool AddTask(TaskItem task)
        {
            if (task == null)
                return false;

            _taskRepo.AddTask(task);
            return true;
        }

        public bool DeleteTask(TaskItem task)
        {
            if (_taskRepo.DeleteTask(task.Id.ToString()))
            {
                return true;
            }

            else return false;
        }
    }
}
