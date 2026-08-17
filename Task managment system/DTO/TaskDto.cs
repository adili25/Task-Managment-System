
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
        public DateTimeOffset DueDate { get; set; }


        [Required(ErrorMessage = "The Created User ID required.")]
        public Guid CreatedByUserId { get; set; }

        
        [Required(ErrorMessage = "The Assigned User ID required.")]
        public Guid AssignedToUserId { get; set; }


        [Required(ErrorMessage = "The Task Priority required.")]
        public Priority Priority { get; set; }
    }
}
