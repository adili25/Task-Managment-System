using Task_Manager.Models;

namespace Task_managment_system.Interfaces
{
    public interface ITaskRepository
    {
        public Task AddTask(TaskItem task, CancellationToken cancellationToken);
        public Task<IQueryable<TaskItem>> GetAllTasks();
        public Task<TaskItem?> GetTaskById(Guid id, CancellationToken cancellationToken);
        public Task DeleteTask(Guid id, CancellationToken cancellationToken);
        public Task SaveChanges(CancellationToken cancellationToken);
    }
}
