using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Configurations
{
    public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.ToTable("PostTags");

            builder.HasKey(pt => new { pt.PostId, pt.TagId });


            builder.HasOne(pt => pt.Post).WithMany(p => p.Tags).HasForeignKey(pt => pt.PostId);

            builder.HasOne(pt => pt.Tag).WithMany(p => p.Posts).HasForeignKey(pt => pt.TagId);
        }
    }
}