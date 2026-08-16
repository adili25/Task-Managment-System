using System.ComponentModel.DataAnnotations;
using Task_managment_system.Enums;

namespace Task_Manager.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Status TaskStatus { get; set; }
        public Priority TaskPriority { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid AssignedToUserId { get; set; }

        public TaskItem(string title, string descreption, Priority priority, DateTime dueDate, Guid createdByUserId, Guid assignedToUserId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = descreption;
            TaskStatus = Status.Pending;
            TaskPriority = priority;
            CreatedAt = DateTime.UtcNow;
            DueDate = dueDate;
            CreatedByUserId = createdByUserId;
            AssignedToUserId = assignedToUserId;
        }
    }
}
