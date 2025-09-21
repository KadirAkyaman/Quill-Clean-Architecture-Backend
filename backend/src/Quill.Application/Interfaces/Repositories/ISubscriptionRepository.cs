using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Domain.Entities;

namespace Quill.Application.Interfaces.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<Subscription?> FindSubscriptionAsync(int subscriberId, int subscribedToId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Subscription>> GetSubscriptionsBySubscriberIdAsync(int subscriberId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Subscription>> GetSubscribersBySubscribedToIdAsync(int subscribedToId, CancellationToken cancellationToken);
        Task AddAsync(Subscription subscription, CancellationToken cancellationToken);
        Task<int> GetSubscriberCountAsync(int userId, CancellationToken cancellationToken);
        Task<int> GetSubscriptionCountAsync(int userId, CancellationToken cancellationToken);
        void Update(Subscription subscription);
    }
}