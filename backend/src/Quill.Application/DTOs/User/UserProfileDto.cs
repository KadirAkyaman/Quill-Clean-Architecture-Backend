using Quill.Application.DTOs.Post;

namespace Quill.Application.DTOs.User
{
    /// <summary>
    /// Represents the full public profile of a user, including stats and recent posts, as returned by the API.
    /// </summary>
    public class UserProfileDto
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        /// <example>15</example>
        public int Id { get; set; }

        /// <summary>
        /// The user's first name.
        /// </summary>
        /// <example>John</example>
        public string Name { get; set; }

        /// <summary>
        /// The user's last name.
        /// </summary>
        /// <example>Doe</example>
        public string Surname { get; set; }

        /// <summary>
        /// The user's unique, public username.
        /// </summary>
        /// <example>johndoe</example>
        public string Username { get; set; }

        /// <summary>
        /// The URL of the user's profile picture. Can be null.
        /// </summary>
        /// <example>https://example.com/images/johndoe.jpg</example>
        public string? ProfilePictureURL { get; set; }

        /// <summary>
        /// The date and time the user registered on the platform.
        /// </summary>
        public DateTime MemberSince { get; set; }

        /// <summary>
        /// The role assigned to the user.
        /// </summary>
        /// <example>Author</example>
        public string Role { get; set; }

        /// <sumnmary>
        /// The user's key statistics, such as post and subscriber counts.
        /// </summary>
        public UserStatsDto Stats { get; set; }

        /// <summary>
        /// A collection of the user's most recent posts for preview.
        /// </summary>
        public ICollection<PostPreviewDto> RecentPosts { get; set; }
    }
}