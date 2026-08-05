using Task_managment_system.Enums;

namespace Task_Manager.Models
{
    public enum Priority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3
    }
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
            if (title == default)
            {
                throw new ArgumentNullException("Null_Title: Must Be A String");
            }
            Title = title;

            if (descreption == default)
            {
                throw new ArgumentNullException("Null_Descreption: Must Be A String");
            }
            Description = descreption;

            if (priority == default)
            {
                throw new ArgumentNullException("Null_Priority: Must Be A String");
            }
            TaskPriority = priority;

            if (DueDate == default)
            {
                throw new ArgumentNullException("Null_DueDate: Must Be A String");
            }
            DueDate = dueDate;

            if (createdByUserId == default)
            {
                throw new ArgumentNullException("Null_CreatedBy: Must Be A String");
            }
            CreatedByUserId = createdByUserId;

            if (assignedToUserId == default)
            {
                throw new ArgumentNullException("Null_AssingedTo: Must Be A String");
            }
            AssignedToUserId = assignedToUserId;

            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            TaskStatus = Status.Pending;
        }
    }
}
