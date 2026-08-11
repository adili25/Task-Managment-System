using System.ComponentModel.DataAnnotations;
using Task_managment_system.Enums;

namespace Task_Manager.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Status TaskStatus { get; set; }
        [Required]
        public Priority TaskPriority { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Required]
        public Guid CreatedByUserId { get; set; }
        [Required]
        public Guid AssignedToUserId { get; set; }

        public TaskItem(string title, string descreption, Priority priority, DateTime dueDate, Guid createdByUserId, Guid assignedToUserId)
        {
            if (title == default)
            {
                throw new ArgumentNullException("Null_Title: Must Be A String");
            }

            if (descreption == default)
            {
                throw new ArgumentNullException("Null_Descreption: Must Be A String");
            }

            if (dueDate == default)
            {
                throw new ArgumentNullException("Null_DueDate: Must Be A String");
            }

            if (createdByUserId == default)
            {
                throw new ArgumentNullException("Null_CreatedBy: Must Be A String");
            }

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
