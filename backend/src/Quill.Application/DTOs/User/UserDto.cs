namespace Quill.Application.DTOs.User
{
    /// <summary>
    /// Represents the standard, detailed view of a user.
    /// </summary>
    public class UserDto
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
        /// The URL of the user's profile picture.
        /// </summary>
        /// <example>https://example.com/images/johndoe.jpg</example>
        public string? ProfilePictureURL { get; set; }

        /// <summary>
        /// The role assigned to the user.
        /// </summary>
        /// <example>Author</example>
        public string Role { get; set; }

        /// <summary>
        /// The date and time the user registered.
        /// </summary>
        public DateTime MemberSince { get; set; }
    }
}