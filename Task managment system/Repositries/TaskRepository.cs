using Task_Manager.Models;

/* 
 * here we define the TaskRepository ot act like the database with shown methods 
*/

namespace Task_managment_system.Repositries
{
    public class TaskRepository
    {
        //acting as the actuall database
        private readonly List<TaskItem> Tasks = [];

        //add task to the database (Tasks)
        public bool AddTask(TaskItem task)
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
        public IQueryable<TaskItem> GetAllTasks()
        {
            return Tasks.AsQueryable();
        }

        //get Task by Id
        public TaskItem? GetTaskById(string id)
        {
            //fetching the user from Users
            TaskItem? task = Tasks.FirstOrDefault(t => t.Id.ToString() == id);
            return task;
        }

        public bool DeleteTask(string id)
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
