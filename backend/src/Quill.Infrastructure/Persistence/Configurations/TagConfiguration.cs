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
        }
    }
}