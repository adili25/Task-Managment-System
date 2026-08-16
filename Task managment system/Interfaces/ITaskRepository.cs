using Task_Manager.Models;

namespace Task_managment_system.Interfaces
{
    public interface ITaskRepository
    {
        public Task<bool> AddTask(TaskItem task);
        public Task<IQueryable<TaskItem>> GetAllTasks();
        public Task<TaskItem?> GetTaskById(string id);
        public Task<bool> DeleteTask(string id);
    }
}
