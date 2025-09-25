namespace Quill.Application.DTOs.User
{
    /// <summary>
    /// Represents the data required for an administrator to change a user's role.
    /// </summary>
    public class AdminUserChangeRoleDto
    {
        /// <summary>
        /// The unique identifier of the new role to assign to the user.
        /// </summary>
        /// <example>2</example>
        public int RoleId { get; set; }
    }
}