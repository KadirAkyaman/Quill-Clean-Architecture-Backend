using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired().HasMaxLength(25);

            builder.HasIndex(t => t.Name).IsUnique();

            builder.HasMany(t => t.Posts).WithOne(pt => pt.Tag).HasForeignKey(pt => pt.TagId).OnDelete(DeleteBehavior.Cascade);

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.HasData(
                new Tag { Id = 1, Name = "CSharp", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Tag { Id = 2, Name = "DotNet", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Tag { Id = 3, Name = "ASPNETCore", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Tag { Id = 4, Name = "Programming", CreatedAt = seedDate, UpdatedAt = seedDate},
                new Tag { Id = 5, Name = "Tutorial", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Tag { Id = 6, Name = "WebDev", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Tag { Id = 7, Name = "AI", CreatedAt = seedDate, UpdatedAt = seedDate }
            );



        }
    }
}