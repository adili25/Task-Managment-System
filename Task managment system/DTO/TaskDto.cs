
/*
 * this class for the upcoming data from the user to prievnt Over-Posting Attach
 */

using System.ComponentModel.DataAnnotations;
using Task_Manager.Models;
using Task_managment_system.Enums;

namespace Task_managment_system.DTO
{
    public class TaskDto
    {
        [Required(ErrorMessage = "The Title required.")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "The Discreption required.")]
        public string Discreption { get; set; }

        [Required(ErrorMessage = "The TaskStatus required.")]
        public Status TaskStatus { get; set; }

        [Required(ErrorMessage = "The Due Date required.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "The Created User ID required.")]
        public Guid CreatedByUserId { get; set; }
        
        [Required(ErrorMessage = "The Assigned User ID required.")]
        public Guid AssignedToUserId { get; set; }

        [Required(ErrorMessage = "The Task Priority required.")]
        public Priority Priority { get; set; }

        public TaskDto(string title, string discreption, Status taskStatus, DateTime dueDate, Guid createdByUserId, Guid assignedToUserId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(discreption))
            {
                throw new ArgumentException("Description cannot be null or empty.", nameof(discreption));
            }

            if (createdByUserId == Guid.Empty)
            {
                throw new ArgumentException("Creator ID cannot be an empty Guid.", nameof(createdByUserId));
            }

            // If all validation passes, assign the values
            Title = title;
            Discreption = discreption;
            TaskStatus = taskStatus;
            DueDate = dueDate;
            CreatedByUserId = createdByUserId;
            AssignedToUserId = assignedToUserId;
        }
    }
}
