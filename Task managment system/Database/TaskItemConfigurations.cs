using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_Manager.Models;

namespace Task_managment_system.Database
{
    public class TaskItemConfigurations : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("Tasks");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).HasMaxLength(200);
            builder.Property(t => t.Description).HasMaxLength(2000);
            builder.Property(t => t.TaskStatus).HasConversion<string>().HasMaxLength(20);
            builder.Property(t => t.TaskPriority).HasConversion<string>().HasMaxLength(20);
            builder.Property(t => t.DueDate);
            builder.Property(t => t.CreatedAt);
            builder.Property(t => t.UpdatedAt);
            builder.HasIndex(t => t.CreatedByUserId);
            builder.HasIndex(t => t.AssignedToUserId);
        }
    }
}
