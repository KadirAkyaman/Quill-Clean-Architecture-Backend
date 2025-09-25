namespace Quill.Application.DTOs.User
{
    /// <summary>
    /// Represents the data an administrator can use to update a user's profile.
    /// </summary>
    public class AdminUserUpdateDto
    {
        /// <summary>
        /// The user's new first name.
        /// </summary>
        /// <example>John</example>
        public string? Name { get; set; }

        /// <summary>
        /// The user's new last name.
        /// </summary>
        /// <example>Doe</example>
        public string? Surname { get; set; }

        /// <summary>
        /// The user's new unique username.
        /// </summary>
        /// <example>johndoe_updated</example>
        public string? Username { get; set; }

        /// <summary>
        /// The user's active status. Use 'true' to activate, 'false' to deactivate.
        /// </summary>
        /// <example>false</example>
        public bool? IsActive { get; set; }
    }
}