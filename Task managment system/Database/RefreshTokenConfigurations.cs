using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_Manager.Models;
using Task_managment_system.Models;

namespace Task_managment_system.Database
{
    public class RefreshTokenConfigurations : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.TokenHash).IsRequired().HasMaxLength(64);
            builder.HasIndex(t => t.TokenHash).IsUnique();
            builder.HasIndex(t => t.UserId);
            builder.Property(t => t.ExpiresAt);
            builder.Property(t => t.CreatedAt);
            builder.Property(t => t.RevokedAt);
            builder.Property(t => t.ReplacedByTokenHash);
            builder.Ignore(t => t.IsExpired);
            builder.Ignore(t => t.IsActive);
            builder.Property<uint>("xmin").IsRowVersion().HasColumnName("xmin");

            builder.HasOne<ApplicationUser>()
                   .WithMany()
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
