namespace Quill.Application.DTOs.Role
{
    /// <summary>
    /// Represents the data required to create a new role.
    /// </summary>
    public class RoleCreateDto
    {
        /// <summary>
        /// The name of the new role. Must be unique and between 2 and 20 characters.
        /// </summary>
        /// <example>Editor</example>
        public string Name { get; set; } = string.Empty;
    }
}