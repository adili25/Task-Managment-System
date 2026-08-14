using Task_Manager.Models;
using Task_managment_system.Interfaces;

/* 
 * here we define the TaskRepository ot act like the database with shown methods 
*/

namespace Task_managment_system.Repositries
{
    public class TaskRepository : ITaskRepository
    {
        //acting as the actuall database
        private readonly List<TaskItem> Tasks = [];

        //add task to the database (Tasks)
        public async Task<bool> AddTask(TaskItem task)
        {
            //check null task
            if (task == default)
            {
                return false;
            }

            //if its not null add it to the database
            Tasks.Add(task);
            return true;
        }

        //get all the Tasks as IEnumerable
        public async Task<IQueryable<TaskItem>> GetAllTasks()
        {
            return Tasks.AsQueryable();
        }

        //get Task by Id
        public async Task<TaskItem?> GetTaskById(string id)
        {
            //fetching the user from Users
            TaskItem? task = Tasks.FirstOrDefault(t => t.Id.ToString() == id);
            return task;
        }

        public async Task<bool> DeleteTask(string id)
        {
            var task = Tasks.FirstOrDefault(t => t.Id.ToString() == id);
            
            if (task == default)
            {
                return false;
            }

            Tasks.Remove(task);
            return true;
        }

    }
}
