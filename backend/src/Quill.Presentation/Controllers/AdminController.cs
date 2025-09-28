using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quill.Application.DTOs.User;
using Quill.Application.Interfaces.Services;

namespace Quill.Presentation.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Tags("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Updates a user's profile information. (Admin Only)
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        /// <param name="adminUserUpdateDto">The data to update for the user.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the update was successful.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="404">If the user to update is not found.</response>
        /// <response code="409">If the new username is already taken.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpPut("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] AdminUserUpdateDto adminUserUpdateDto, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserByAdminAsync(userId, adminUserUpdateDto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Changes a user's role. (Admin Only)
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose role is to be changed.</param>
        /// <param name="changeRoleDto">The DTO containing the new Role ID.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the role change was successful.</response>
        /// <response code="400">If the provided Role ID is invalid or does not exist.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpPut("{userId:int}/role")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangeUserRole(int userId, [FromBody] AdminUserChangeRoleDto changeRoleDto, CancellationToken cancellationToken)
        {
            await _userService.ChangeUserRoleByAdminAsync(userId, changeRoleDto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deactivates a user account (soft delete). (Admin Only)
        /// </summary>
        /// <param name="userId">The unique identifier of the user to deactivate.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the deactivation was successful.</response>
        /// <response code="404">If the user to deactivate is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpDelete("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserByAdminAsync(userId, cancellationToken);
            return NoContent();
        }
        
        /// <summary>
        /// Retrieves a standard, detailed view of a user by their unique identifier. (Admin Only)
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The detailed information for the specified user.</returns>
        /// <response code="200">Returns the requested user's details.</response>
        /// <response code="404">If a user with the specified ID is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpGet("{userId:int}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDto>> GetById(int userId, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}