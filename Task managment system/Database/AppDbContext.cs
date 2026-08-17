using Microsoft.EntityFrameworkCore;
using Task_Manager.Models;
using System.Reflection;

namespace Task_managment_system.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; } = null!;
        public DbSet<ApplicationUser> Users { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> builder) : base(builder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


    }
}
