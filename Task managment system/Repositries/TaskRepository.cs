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
        public void AddTask(TaskItem task)
        {
            //check null task
            if (task == default)
            {
                throw new NullReferenceException("TASK_IS_NULL");
            }

            //if its not null add it to the database
            Tasks.Add(task);
        }

        //get all the Tasks as IEnumerable
        public IEnumerable<TaskItem> GetAllTasks()
        {
            //--here should me return it using yield return?--
            return Tasks.AsEnumerable();
        }

        //get Task by Id
        public TaskItem GetTaskById(string Id)
        {
            //fetching the user from Users
            TaskItem? task = Tasks.FirstOrDefault(t => t.Id.ToString() == Id);

            //check if the user exist or not
            if (task == default)
            {
                throw new NullReferenceException("TASK_IS_NULL");
            }

            return task;
        }

    }
}
