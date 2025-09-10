using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.IsActive).IsRequired().HasDefaultValue(true); ;

            builder.HasIndex(s => new { s.SubscriberId, s.SubscribedToId }).IsUnique();

            builder.HasOne(s => s.Subscriber).WithMany(u => u.Subscriptions).HasForeignKey(s => s.SubscriberId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.SubscribedTo).WithMany(u => u.Subscribers).HasForeignKey(s => s.SubscribedToId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}