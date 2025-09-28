namespace Quill.Application.DTOs.User
{
    /// <summary>
    /// Represents the response returned after a successful authentication (login or register).
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// The authenticated user's unique identifier.
        /// </summary>
        /// <example>15</example>
        public int Id { get; set; }
        
        /// <summary>
        /// The JWT Bearer token for the user's session.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// The expiration date and time of the token.
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// The username of the authenticated user.
        /// </summary>
        /// <example>johndoe</example>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The role of the authenticated user.
        /// </summary>
        /// <example>Author</example>
        public string Role { get; set; } = string.Empty;
    }
}