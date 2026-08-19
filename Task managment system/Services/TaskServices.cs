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

        private bool IsNotAuthorized(TaskItem task, Guid currentUserId, bool isAdmin)
        {
            return (task.CreatedByUserId != currentUserId && task.AssignedToUserId != currentUserId && !isAdmin);
        }

        public async Task<IQueryable<TaskItem>> GetFilteredTasks(Guid userId, bool isAdmin, TaskFilters filters)
        {
            var tasks = await _taskRepo.GetAllTasks();

            if (!isAdmin)
            {
                tasks = tasks.Where(t => t.AssignedToUserId == userId);
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

            if (filters.AssignedId != null)
            {
                tasks = tasks.Where(t => t.AssignedToUserId == filters.AssignedId);
            }

            if (filters.CreatorId != null)
            {
                tasks = tasks.Where(t => t.CreatedByUserId == filters.CreatorId);
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

        public async Task<TaskItem> GetTaskById(Guid id, Guid currentUserId, bool isAdmin, CancellationToken cancellationToken)
        {
            var task = await _taskRepo.GetTaskById(id, cancellationToken);

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

        public async Task<TaskItem> AddTask(TaskItem task, CancellationToken cancellationToken)
        {
            await _taskRepo.AddTask(task, cancellationToken);
            return task;
        }

        public async Task DeleteTask(TaskItem task, CancellationToken cancellationToken)
        {
            await _taskRepo.DeleteTask(task.Id, cancellationToken);
        }

        public async Task UpdateTask(TaskItem task, UpdateTaskDto updatedTask, CancellationToken cancellationToken)
        {
            if (updatedTask.Title is not null)
            {
                task.Title = updatedTask.Title;
            }

            if (updatedTask.Discreption is not null)
            {
                task.Description = updatedTask.Discreption;
            }

            if (updatedTask.TaskStatus is not null)
            {
                task.TaskStatus = updatedTask.TaskStatus.Value;
            }

            if (updatedTask.DueDate is not null)
            {
                task.DueDate = updatedTask.DueDate.Value;
            }

            if (updatedTask.AssignedToUserId is not null)
            {
                task.AssignedToUserId = updatedTask.AssignedToUserId.Value;
            }

            if (updatedTask.Priority is not null)
            {
                task.TaskPriority = updatedTask.Priority.Value;
            }

            task.UpdatedAt = DateTimeOffset.UtcNow;

            await _taskRepo.SaveChanges(cancellationToken);
        }
    }
}
