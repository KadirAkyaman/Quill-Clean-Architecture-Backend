using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);

            builder.Property(p => p.Summary).IsRequired().HasMaxLength(300);

            builder.Property(p => p.Text).IsRequired();

            builder.Property(p => p.Status).IsRequired().HasConversion<string>().HasMaxLength(20); //Although Status is an enum in the code, when it is saved to the database, save its name as a string, not its numerical value.


            builder.HasOne(p => p.User).WithMany(u => u.Posts).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Category).WithMany(c => c.Posts).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Tags).WithOne(pt => pt.Post).HasForeignKey(pt => pt.PostId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}