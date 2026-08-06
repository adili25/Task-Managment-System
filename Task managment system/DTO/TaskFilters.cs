using System.ComponentModel.DataAnnotations;
using Task_Manager.Models;
using Task_managment_system.Enums;

namespace Task_managment_system.DTO
{
    public class TaskFilters
    {
        [Required]
        public string TitleOrDescription {get; set;}
        public Status? Status { get; set; } = null;
        public Priority? Priority { get; set; } = null;
        public string? AssignedId { get; set; } = null;
        public string? CreatorId { get; set; } = null;
        public DateTime? FromDueDate { get; set; } = null;
        public DateTime? ToDueDate { get; set; } = null;

        public TaskFilters(
            string titleOrDescription,
            Status? status = null,
            Priority? priority = null,
            string? assignedId = null,
            string? creatorId = null,
            DateTime? fromDueDate = null,
            DateTime? toDueDate = null)
        {
            TitleOrDescription = titleOrDescription;
            Status = status;
            Priority = priority;
            AssignedId = assignedId;
            CreatorId = creatorId;
            FromDueDate = fromDueDate;
            ToDueDate = toDueDate;
        }


    }
}
