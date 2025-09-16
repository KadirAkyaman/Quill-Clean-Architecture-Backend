using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quill.Application.Interfaces.Repositories;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _context;
        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken)
        {
            await _context.Subscriptions.AddAsync(subscription, cancellationToken);
        }

        public async Task<Subscription?> FindSubscriptionAsync(int subscriberId, int subscribedToId, CancellationToken cancellationToken)
        {
            return await _context.Subscriptions.AsNoTracking().SingleOrDefaultAsync(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId, cancellationToken);
        }

        public async Task<IReadOnlyList<Subscription>> GetSubscribersBySubscribedToIdAsync(int subscribedToId, CancellationToken cancellationToken)
        {
            return await _context.Subscriptions.AsNoTracking().Where(s => s.SubscribedToId == subscribedToId).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Subscription>> GetSubscriptionsBySubscriberIdAsync(int subscriberId, CancellationToken cancellationToken)
        {
            return await _context.Subscriptions.AsNoTracking().Where(s => s.SubscriberId == subscriberId).ToListAsync(cancellationToken);
        }

        public void Remove(Subscription subscription)
        {
            _context.Subscriptions.Remove(subscription);
        }

        public void Update(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
        }
    }
}