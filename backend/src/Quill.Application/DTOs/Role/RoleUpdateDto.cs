namespace Quill.Application.DTOs.Role
{
    /// <summary>
    /// Represents the data required to update an existing role.
    /// Only non-null properties will be updated.
    /// </summary>
    public class RoleUpdateDto
    {
        /// <summary>
        /// The new name for the role. If provided, must be unique and between 2 and 20 characters.
        /// </summary>
        /// <example>Content Editor</example>
        public string? Name { get; set; }
    }
}