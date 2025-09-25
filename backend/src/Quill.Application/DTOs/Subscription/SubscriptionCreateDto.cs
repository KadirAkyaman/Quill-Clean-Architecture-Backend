namespace Quill.Application.DTOs.Subscription
{
    /// <summary>
    /// Represents the data required to create a new subscription.
    /// </summary>
    public class SubscriptionCreateDto
    {
        /// <summary>
        /// The unique identifier of the user to subscribe to.
        /// </summary>
        /// <example>25</example>
        public int SubscribedToId { get; set; }
    }
}