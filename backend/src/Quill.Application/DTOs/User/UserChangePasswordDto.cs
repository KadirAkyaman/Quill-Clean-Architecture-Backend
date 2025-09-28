namespace Quill.Application.DTOs.User
{
    /// <summary>
    /// Represents the data required for a user to change their password.
    /// </summary>
    public class UserChangePasswordDto
    {
        /// <summary>
        /// The user's current, valid password.
        /// </summary>
        /// <example>OldPassword123!</example>
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// The user's desired new password. Must meet complexity requirements.
        /// </summary>
        /// <example>NewPassword456!</example>
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Confirmation of the new password. Must match NewPassword.
        /// </summary>
        /// <example>NewPassword456!</example>
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}