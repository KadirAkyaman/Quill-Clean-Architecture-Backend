using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Application.DTOs.Subscription;

namespace Quill.Application.Interfaces.Services
{
    public interface ISubscriptionService
    {
        // Finds the subscription relationship between two users.
        // The result can be one of three states:
        // 1. Returns null if no subscription has ever existed.
        // 2. Returns a Subscription object with IsActive = true if the user is currently subscribed.
        // 3. Returns a Subscription object with IsActive = false if the user has unsubscribed (soft delete).
        //------------This method is critical to prevent a user from subscribing to the same person again.-------------
        Task<SubscriptionDto> SubscribeAsync(int subscriberId, SubscriptionCreateDto subscriptionCreateDto, CancellationToken cancellationToken);
        Task UnsubscribeAsync(int subscriberId, int subscribedToId, CancellationToken cancellationToken);
        Task<IReadOnlyList<SubscriptionDto>> GetSubscriptionsAsync(int subscriberId, CancellationToken cancellationToken);
        Task<IReadOnlyList<SubscriptionDto>> GetSubscribersAsync(int subscribedToId, CancellationToken cancellationToken);
    }
}