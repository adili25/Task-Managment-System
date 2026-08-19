using System.ComponentModel.DataAnnotations;
using Task_Manager.Models;
using Task_managment_system.Enums;

namespace Task_managment_system.DTO
{
    public class TaskFilters
    {
        public string? TitleOrDescription { get; set; } = null;
        public Status? Status { get; set; } = null;
        public Priority? Priority { get; set; } = null;
        public Guid? AssignedId { get; set; } = null;
        public Guid? CreatorId { get; set; } = null;
        public DateTimeOffset? FromDueDate { get; set; } = null;
        public DateTimeOffset? ToDueDate { get; set; } = null;

        public TaskFilters() { }

        public TaskFilters(
            string titleOrDescription,
            Status? status = null,
            Priority? priority = null,
            Guid? assignedId = null,
            Guid? creatorId = null,
            DateTimeOffset? fromDueDate = null,
            DateTimeOffset? toDueDate = null)
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
