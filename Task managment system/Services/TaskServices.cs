using Task_Manager.Models;
using Task_managment_system.DTO;
using Task_managment_system.Interfaces;
using Task_managment_system.Exceptions;

namespace Task_managment_system.Services
{
    public class TaskServices
    {
        private readonly ITaskRepository _taskRepo;

        public TaskServices(ITaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        private bool IsNotAuthorized(TaskItem task, string currentUserId, bool isAdmin)
        {
            string createdByUserId = task.CreatedByUserId.ToString();
            string assignedToUserId = task.AssignedToUserId.ToString();

            return (createdByUserId != currentUserId && assignedToUserId != currentUserId && !isAdmin);
        }

        public async Task<IQueryable<TaskItem>> GetFilteredTasks(string userId, bool isAdmin, TaskFilters filters)
        {
            var tasks = await _taskRepo.GetAllTasks();

            if (!isAdmin)
            {
                tasks = tasks.Where(t => t.AssignedToUserId.ToString() == userId);
            }

            if (filters.TitleOrDescription != null)
            {
                tasks = tasks.Where(t => t.Title.Contains(filters.TitleOrDescription) || t.Description.Contains(filters.TitleOrDescription));
            }

            if (filters.Status != null)
            {
                tasks = tasks.Where(t => t.TaskStatus == filters.Status);
            }

            if (filters.Priority != null)
            {
                tasks = tasks.Where(t => t.TaskPriority == filters.Priority);
            }

            if (!string.IsNullOrEmpty(filters.AssignedId))
            {
                tasks = tasks.Where(t => t.AssignedToUserId.ToString() == filters.AssignedId);
            }

            if (!string.IsNullOrEmpty(filters.CreatorId))
            {
                tasks = tasks.Where(t => t.CreatedByUserId.ToString() == filters.CreatorId);
            }

            if (filters.FromDueDate != null)
            {
                tasks = tasks.Where(t => t.DueDate >= filters.FromDueDate);
            }

            if (filters.ToDueDate != null)
            {
                tasks = tasks.Where(t => t.DueDate <= filters.ToDueDate);
            }

            return tasks;
        }

        public async Task<TaskItem> GetTaskById(string id, string currentUserId, bool isAdmin)
        {
            var task = await _taskRepo.GetTaskById(id);

            if (task is null)
            {
                throw new NotFoundException($"task with id:{id} not found");
            }

            if (IsNotAuthorized(task, currentUserId, isAdmin))
            {
                throw new ForbiddenException("user dont is not authorized");
            }

            return task;
        }

        public async Task<TaskItem> AddTask(TaskItem task)
        {
            await _taskRepo.AddTask(task);
            return task;
        }

        public async Task<bool> DeleteTask(TaskItem task)
        {
            if (await _taskRepo.DeleteTask(task.Id.ToString()))
            {
                return true;
            }

            else return false;
        }

        public async Task UpdateTask(TaskItem task, TaskDto updatedTask)
        {
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Discreption;
            task.TaskStatus = updatedTask.TaskStatus;
            task.DueDate = updatedTask.DueDate;
            task.AssignedToUserId = updatedTask.AssignedToUserId;
            task.TaskPriority = updatedTask.Priority;
            task.UpdatedAt = DateTime.UtcNow;
        }
    }
}
