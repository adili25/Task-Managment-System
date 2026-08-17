using System.ComponentModel.DataAnnotations;
using Task_managment_system.Enums;

namespace Task_Manager.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Status TaskStatus { get; set; }
        public Priority TaskPriority { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid AssignedToUserId { get; set; }

        public TaskItem(string title, string descreption, Priority priority, DateTimeOffset dueDate, Guid createdByUserId, Guid assignedToUserId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = descreption;
            TaskStatus = Status.Pending;
            TaskPriority = priority;
            CreatedAt = DateTimeOffset.UtcNow;
            DueDate = dueDate;
            CreatedByUserId = createdByUserId;
            AssignedToUserId = assignedToUserId;
        }

        //----> add param less contstructor for EF
        public TaskItem() { }
    }
}
