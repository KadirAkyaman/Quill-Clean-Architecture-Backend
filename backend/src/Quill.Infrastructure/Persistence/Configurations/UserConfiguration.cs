using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name).IsRequired().HasMaxLength(30);
            builder.Property(u => u.Surname).IsRequired().HasMaxLength(30);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(30);
            builder.Property(u => u.Username).IsRequired().HasMaxLength(15);
            builder.Property(u => u.PasswordHash).IsRequired();

            builder.Property(u => u.ProfilePictureURL).IsRequired(false).HasMaxLength(2048);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Username).IsUnique();


            builder.HasMany(u => u.Posts).WithOne(p => p.User).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Subscriptions).WithOne(s => s.Subscriber).HasForeignKey(s => s.SubscriberId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Subscribers).WithOne(s => s.SubscribedTo).HasForeignKey(s => s.SubscribedToId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}