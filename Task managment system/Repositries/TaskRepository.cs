using Task_Manager.Models;
using Task_managment_system.Interfaces;
using Task_managment_system.Database;
using Task_managment_system.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;


/* 
 * here we define the TaskRepository ot act like the database with shown methods 
*/

namespace Task_managment_system.Repositries
{
    public class TaskRepository : ITaskRepository
    {
        //acting as the actuall database
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        //add task to the database (Tasks)
        public async Task AddTask(TaskItem task, CancellationToken cancellationToken)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken);
        }

        //get all the Tasks as IEnumerable
        public async Task<IQueryable<TaskItem>> GetAllTasks()
        {
            return _context.Tasks;
        }

        //get Task by Id
        public async Task<TaskItem?> GetTaskById(Guid id, CancellationToken cancellationToken)
        {
            //fetching the user from Users
            var task =await _context.Tasks.FindAsync(new object?[] { id }, cancellationToken);
            return task;
        }

        public async Task DeleteTask(Guid id, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.FindAsync(new object?[] { id }, cancellationToken);
            if (task is null)
            {
                throw new NotFoundException($"the user id: {id} not found for delete");
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
