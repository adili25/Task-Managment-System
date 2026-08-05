using Task_Manager.Models;
using Task_managment_system.Enums;

namespace Task_managment_system.DTO
{
    public class TaskFilters
    {
        public Dictionary<string, string> TitleOrDiscription { get; set; }
        //TitleOrDiscription<"title", "actual title">
        //TitleOrDiscription<"discreption", "actual discreption">
        public Status? Status { get; set; } = null;
        public Priority? Priority { get; set; } = null;
        public string? AssignedId { get; set; } = null;
        public string? CreatorId { get; set; } = null;
        public DateTime? FromDueDate { get; set; } = null;
        public DateTime? ToDueDate { get; set; } = null;


        // 1. Parameterless constructor (Required by ASP.NET Core Model Binding)
        public TaskFilters(){ }

        // 2. Parameterized constructor with optional arguments
        public TaskFilters(
            string? title = null,
            string? description = null,
            Status? status = null,
            Priority? priority = null,
            string? assignedId = null,
            string? creatorId = null,
            DateTime? fromDueDate = null,
            DateTime? toDueDate = null)
        {
            Title = title;
            this.description = description; // Uses 'this.' to distinguish from the parameter
            Status = status;
            Priority = priority;
            AssignedId = assignedId;
            CreatorId = creatorId;
            FromDueDate = fromDueDate;
            ToDueDate = toDueDate;
        }


    }
}
