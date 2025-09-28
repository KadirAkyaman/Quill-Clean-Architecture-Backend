using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name).IsRequired().HasMaxLength(20);

            builder.HasIndex(r => r.Name).IsUnique();

            builder.HasMany(r => r.Users).WithOne(u => u.Role).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.HasData(
                new Role { Id = 1, Name = "Admin", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Role { Id = 2, Name = "Author", CreatedAt = seedDate, UpdatedAt = seedDate }
            );
        }
    }
}