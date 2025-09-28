using Quill.Application.DTOs.User;

namespace Quill.Application.DTOs.Subscription
{
    /// <summary>
    /// Represents a subscription relationship as returned by the API.
    /// </summary>
    public class SubscriptionDto
    {
        /// <summary>
        /// The unique identifier for the subscription record itself.
        /// </summary>
        /// <example>101</example>
        public int Id { get; set; }

        /// <summary>
        /// A summary of the user who is subscribing.
        /// </summary>
        public UserSummaryDto Subscriber { get; set; } = null!;

        /// <summary>
        /// A summary of the user who is being subscribed to.
        /// </summary>
        public UserSummaryDto SubscribedTo { get; set; } = null!;
        
        /// <summary>
        /// The date and time the subscription was initiated.
        /// </summary>
        public DateTime SubscriptionDate { get; set; }
    }
}