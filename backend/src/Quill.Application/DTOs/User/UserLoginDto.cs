namespace Quill.Application.DTOs.User
{
    /// <summary>
    /// Represents the credentials required for a user to log in.
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// The user's registered email address.
        /// </summary>
        /// <example>john.doe@example.com</example>
        public string Email { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        /// <example>Password123!</example>
        public string Password { get; set; }
    }
}