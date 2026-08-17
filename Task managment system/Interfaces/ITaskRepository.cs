using Task_Manager.Models;

namespace Task_managment_system.Interfaces
{
    public interface ITaskRepository
    {
        public Task AddTask(TaskItem task);
        public Task<IQueryable<TaskItem>> GetAllTasks();
        public Task<TaskItem?> GetTaskById(Guid id);
        public Task DeleteTask(Guid id);
        public Task SaveChanges();
    }
}
