using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired().HasMaxLength(40);

            builder.HasIndex(c => c.Name).IsUnique();


            builder.HasMany(c => c.Posts).WithOne(p => p.Category).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.HasData(
                new Category { Id = 1, Name = "Technology", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { Id = 2, Name = "SoftwareDevelopment", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { Id = 3, Name = "Productivity", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { Id = 4, Name = "Lifestyle", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { Id = 5, Name = "Science", CreatedAt = seedDate, UpdatedAt = seedDate }
            );
        }
    }
}