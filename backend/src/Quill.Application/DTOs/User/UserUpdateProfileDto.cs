namespace Quill.Application.DTOs.User
{
    /// <summary>
    /// Represents the data that a logged-in user can update on their own profile.
    /// Only non-null properties will be updated.
    /// </summary>
    public class UserUpdateProfileDto
    {
        /// <summary>
        /// The user's new first name.
        /// </summary>
        /// <example>Jane</example>
        public string? Name { get; set; }

        /// <summary>
        /// The user's new last name.
        /// </summary>
        /// <example>Smith</example>
        public string? Surname { get; set; }

        /// <summary>
        /// The user's new unique username.
        /// </summary>
        /// <example>janesmith</example>
        public string? Username { get; set; }

        /// <summary>
        /// The new URL for the user's profile picture.
        /// </summary>
        /// <example>https://example.com/images/janesmith.jpg</example>
        public string? ProfilePictureURL { get; set; }
    }
}