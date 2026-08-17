using System.ComponentModel.DataAnnotations;
using Task_Manager.Models;
using Task_managment_system.Enums;

namespace Task_managment_system.DTO
{
    public class UpdateTaskDto
    {
        public string? Title { get; set; }
        public string? Discreption { get; set; }
        public Status? TaskStatus { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public Priority? Priority { get; set; }
    }
}

