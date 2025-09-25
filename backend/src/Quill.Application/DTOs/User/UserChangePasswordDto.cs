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
        public string CurrentPassword { get; set; }

        /// <summary>
        /// The user's desired new password. Must meet complexity requirements.
        /// </summary>
        /// <example>NewPassword456!</example>
        public string NewPassword { get; set; }

        /// <summary>
        /// Confirmation of the new password. Must match NewPassword.
        /// </summary>
        /// <example>NewPassword456!</example>
        public string ConfirmNewPassword { get; set; }
    }
}