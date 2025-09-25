using Quill.Application.DTOs.Subscription;
using Quill.Application.Exceptions;

namespace Quill.Application.Interfaces.Services
{
    /// <summary>
    /// Defines business logic operations for managing user subscriptions.
    /// </summary>
    public interface ISubscriptionService
    {
        /// <summary>
        /// Subscribes the authenticated user to another user.
        /// If a previous, inactive subscription exists, it will be reactivated.
        /// </summary>
        /// <param name="subscriberId">The ID of the user performing the action (the follower).</param>
        /// <param name="createDto">The DTO containing the ID of the user to be followed.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the DTO of the created or reactivated subscription.</returns>
        /// <exception cref="InvalidSubscriptionOperationException">Thrown if a user attempts to subscribe to themselves.</exception>
        /// <exception cref="AlreadySubscribedException">Thrown if the user is already actively subscribed.</exception>
        /// <exception cref="NotFoundException">Thrown if the user to subscribe to does not exist.</exception>
        Task<SubscriptionDto> SubscribeAsync(int subscriberId, SubscriptionCreateDto createDto, CancellationToken cancellationToken);

        /// <summary>
        /// Unsubscribes the authenticated user from another user.
        /// This is a soft-delete operation, setting the subscription to inactive.
        /// </summary>
        /// <param name="subscriberId">The ID of the user performing the action (the follower).</param>
        /// <param name="subscribedToId">The ID of the user being unfollowed.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidSubscriptionOperationException">Thrown if a user attempts to unsubscribe from themselves.</exception>
        Task UnsubscribeAsync(int subscriberId, int subscribedToId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Retrieves a list of users that a specific user is subscribed to (their "following" list).
        /// </summary>
        /// <param name="subscriberId">The ID of the user whose "following" list is to be retrieved.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of Subscription DTOs.</returns>
        Task<IReadOnlyList<SubscriptionDto>> GetSubscriptionsAsync(int subscriberId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a list of users who are subscribed to a specific user (their "followers" list).
        /// </summary>
        /// <param name="subscribedToId">The ID of the user whose "followers" list is to be retrieved.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of Subscription DTOs.</returns>
        Task<IReadOnlyList<SubscriptionDto>> GetSubscribersAsync(int subscribedToId, CancellationToken cancellationToken);
    }
}