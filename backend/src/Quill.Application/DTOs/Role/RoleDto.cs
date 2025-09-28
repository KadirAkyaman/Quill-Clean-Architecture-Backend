namespace Quill.Application.DTOs.Role
{
    /// <summary>
    /// Represents a role as returned by the API.
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// The unique identifier for the role.
        /// </summary>
        /// <example>3</example>
        public int Id { get; set; }

        /// <summary>
        /// The name of the role.
        /// </summary>
        /// <example>Editor</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The number of users assigned to this role.
        /// </summary>
        /// <example>12</example>
        public int UserCount { get; set; }
    }
}