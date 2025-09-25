using Quill.Application.DTOs.User;
using Quill.Application.Exceptions;

namespace Quill.Application.Interfaces.Services
{
    /// <summary>
    /// Defines business logic operations for User entities, including authentication, profile management, and administration.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="registerDto">The DTO containing the new user's registration details.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing an authentication response with a JWT upon success.</returns>
        /// <exception cref="ConflictException">Thrown if the email or username is already in use.</exception>
        Task<AuthResponseDto> RegisterAsync(UserRegisterDto registerDto, CancellationToken cancellationToken);

        /// <summary>
        /// Authenticates a user and provides a JWT for subsequent requests.
        /// </summary>
        /// <param name="loginDto">The DTO containing the user's login credentials.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing an authentication response with a JWT upon success.</returns>
        /// <exception cref="UnauthorizedActionException">Thrown if the provided credentials are invalid.</exception>
        Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a summary list of all users.
        /// </summary>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of User Summary DTOs.</returns>
        Task<IReadOnlyList<UserSummaryDto>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a standard, detailed view of a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the User DTO, or null if not found.</returns>
        Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the full public profile of a user by their unique username.
        /// </summary>
        /// <param name="username">The unique username of the user.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the User Profile DTO, or null if not found.</returns>
        Task<UserProfileDto?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        
        /// <summary>
        /// Updates the profile information of the currently authenticated user.
        /// </summary>
        /// <param name="userId">The ID of the authenticated user performing the update.</param>
        /// <param name="updateDto">The DTO containing the new profile data.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
        /// <exception cref="ConflictException">Thrown if the new username is already taken by another user.</exception>
        Task UpdateProfileAsync(int userId, UserUpdateProfileDto updateDto, CancellationToken cancellationToken);

        /// <summary>
        /// Changes the password for the currently authenticated user.
        /// </summary>
        /// <param name="userId">The ID of the authenticated user changing their password.</param>
        /// <param name="changePasswordDto">The DTO containing the current and new password information.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
        /// <exception cref="UnauthorizedActionException">Thrown if the provided current password is incorrect.</exception>
        Task ChangePasswordAsync(int userId, UserChangePasswordDto changePasswordDto, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a user's information. This action can only be performed by an administrator.
        /// </summary>
        /// <param name="userId">The ID of the user to be updated.</param>
        /// <param name="updateDto">The DTO containing the data to update.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
        /// <exception cref="ConflictException">Thrown if the new username is already taken by another user.</exception>
        Task UpdateUserByAdminAsync(int userId, AdminUserUpdateDto updateDto, CancellationToken cancellationToken);

        /// <summary>
        /// Changes a user's role. This action can only be performed by an administrator.
        /// </summary>
        /// <param name="userId">The ID of the user whose role is to be changed.</param>
        /// <param name="changeRoleDto">The DTO containing the new Role ID.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the user or the specified role is not found.</exception>
        Task ChangeUserRoleByAdminAsync(int userId, AdminUserChangeRoleDto changeRoleDto, CancellationToken cancellationToken);

        /// <summary>
        /// Deactivates a user account (soft delete). This action can only be performed by an administrator.
        /// </summary>
        /// <param name="userId">The ID of the user to deactivate.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
        Task DeleteUserByAdminAsync(int userId, CancellationToken cancellationToken);
    }
}