namespace Quill.Application.DTOs.User
{
    /// <summary>
    /// Represents a user's key statistics.
    /// </summary>
    public class UserStatsDto
    {
        /// <summary>
        /// The total number of posts written by the user.
        /// </summary>
        /// <example>42</example>
        public int PostsCount { get; set; }

        /// <summary>
        /// The total number of users who are subscribed to this user (followers).
        /// </summary>
        /// <example>128</example>
        public int SubscriberCount { get; set; }
        
        /// <summary>
        /// The total number of users this user is subscribed to (following).
        /// </summary>
        /// <example>32</example>
        public int SubscriptionsCount { get; set; }
    }
}