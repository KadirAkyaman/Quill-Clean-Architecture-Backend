namespace Quill.Application.DTOs.User
{
    /// <summary>
    /// Represents the data required to register a new user.
    /// </summary>
    public class UserRegisterDto
    {
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
        /// The user's email address. Must be a valid email format and unique.
        /// </summary>
        /// <example>john.doe@example.com</example>
        public string Email { get; set; }

        /// <summary>
        /// The user's desired username. Must be unique.
        /// </summary>
        /// <example>johndoe</example>
        public string Username { get; set; }

        /// <summary>
        /// The user's password. Must meet complexity requirements.
        /// </summary>
        /// <example>Password123!</example>
        public string Password { get; set; }
        
        /// <summary>
        /// Confirmation of the user's password. Must match the Password field.
        /// </summary>
        /// <example>Password123!</example>
        public string ConfirmPassword { get; set; }
    }
}