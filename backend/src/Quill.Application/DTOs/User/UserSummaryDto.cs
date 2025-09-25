namespace Quill.Application.DTOs.User
{
    /// <summary>
    /// Represents a summarized, public view of a user, typically for lists or previews.
    /// </summary>
    public class UserSummaryDto
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        /// <example>15</example>
        public int Id { get; set; }
        
        /// <summary>
        /// The user's unique username.
        /// </summary>
        /// <example>johndoe</example>
        public string Username { get; set; }
        
        /// <summary>
        /// The user's full name.
        /// </summary>
        /// <example>John Doe</example>
        public string Name { get; set; }
        
        /// <summary>
        /// The URL of the user's profile picture.
        /// </summary>
        /// <example>https://example.com/images/johndoe.jpg</example>
        public string? ProfilePictureUrl { get; set; }
    }
}