using Quill.Domain.Entities;

namespace Quill.Application.Interfaces.Repositories
{
    /// <summary>
    /// Defines the contract for data operations on the Subscription entity.
    /// </summary>
    public interface ISubscriptionRepository
    {
        /// <summary>
        /// Finds the specific subscription relationship between two users.
        /// This method is crucial to check the state (active, inactive, or non-existent) of a subscription.
        /// </summary>
        /// <param name="subscriberId">The ID of the user who is subscribing.</param>
        /// <param name="subscribedToId">The ID of the user being subscribed to.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the Subscription entity if a relationship exists (active or inactive); otherwise, null.</returns>
        Task<Subscription?> FindSubscriptionAsync(int subscriberId, int subscribedToId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a list of subscriptions made by a specific user (who they are following).
        /// </summary>
        /// <param name="subscriberId">The ID of the user whose subscriptions are to be retrieved.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a read-only list of subscriptions.</returns>
        Task<IReadOnlyList<Subscription>> GetSubscriptionsBySubscriberIdAsync(int subscriberId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a list of subscribers for a specific user (their followers).
        /// </summary>
        /// <param name="subscribedToId">The ID of the user whose subscribers are to be retrieved.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a read-only list of subscriptions.</returns>
        Task<IReadOnlyList<Subscription>> GetSubscribersBySubscribedToIdAsync(int subscribedToId, CancellationToken cancellationToken);

        /// <summary>
        /// Calculates the total number of active subscribers for a specific user (their follower count).
        /// </summary>
        /// <param name="userId">The ID of the user whose subscribers are to be counted.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total number of subscribers.</returns>
        Task<int> GetSubscriberCountAsync(int userId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Calculates the total number of active subscriptions a specific user has made (their following count).
        /// </summary>
        /// <param name="userId">The ID of the user whose subscriptions are to be counted.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total number of subscriptions.</returns>
        Task<int> GetSubscriptionCountAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Marks a new subscription to be added to the database.
        /// </summary>
        /// <param name="subscription">The new subscription entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(Subscription subscription, CancellationToken cancellationToken);

        /// <summary>
        /// Marks an existing subscription as modified in the change tracker (e.g., to handle soft deletes).
        /// </summary>
        /// <param name="subscription">The subscription entity to update.</param>
        void Update(Subscription subscription);
    }
}