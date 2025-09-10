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
        }
    }
}