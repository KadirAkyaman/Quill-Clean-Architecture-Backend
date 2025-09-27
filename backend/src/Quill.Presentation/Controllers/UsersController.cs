using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quill.Application.DTOs.Subscription;
using Quill.Application.DTOs.User;
using Quill.Application.Interfaces.Services;

namespace Quill.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISubscriptionService _subscriptionService;

        public UsersController(IUserService userService, ISubscriptionService subscriptionService)
        {
            _userService = userService;
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Retrieves a summary list of all users.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of all users with summary information.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<UserSummaryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<UserSummaryDto>>> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllAsync(cancellationToken);
            return Ok(users);
        }

        /// <summary>
        /// Retrieves the full public profile of a user by their unique username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The full public profile of the specified user.</returns>
        /// <response code="200">Returns the requested user's profile.</response>
        /// <response code="404">If a user with the specified username is not found.</response>
        [HttpGet("{username}")]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDto>> GetByUsername(string username, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByUsernameAsync(username, cancellationToken);
            if (user is null)
            {
                return NotFound($"User with username '{username}' not found.");
            }
            return Ok(user);
        }

        /// <summary>
        /// Retrieves the profile of the currently authenticated user.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The profile data for the logged-in user.</returns>
        /// <response code="200">Returns the user's profile.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the user's profile cannot be found.</response>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentUserProfile(CancellationToken cancellationToken)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }
            var userProfileByName = await _userService.GetByUsernameAsync(username, cancellationToken);
            if (userProfileByName is null)
            {
                return NotFound();
            }
            return Ok(userProfileByName);
        }

        /// <summary>
        /// Updates the profile of the currently authenticated user.
        /// </summary>
        /// <param name="userUpdateProfileDto">The DTO containing the new profile data.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the update was successful.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the user's profile is not found.</response>
        /// <response code="409">If the new username is already taken.</response>
        [HttpPut("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateProfileDto userUpdateProfileDto, CancellationToken cancellationToken)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString);
            await _userService.UpdateProfileAsync(userId, userUpdateProfileDto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Changes the password for the currently authenticated user.
        /// </summary>
        /// <param name="userChangePasswordDto">The DTO containing the current and new password information.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the password was changed successfully.</response>
        /// <response code="400">If the provided data is invalid (e.g., new passwords do not match).</response>
        /// <response code="401">If the user is not authenticated or the current password is incorrect.</response>
        [HttpPut("me/password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString);
            await _userService.ChangePasswordAsync(userId, userChangePasswordDto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Subscribes the currently authenticated user to another user.
        /// </summary>
        /// <param name="username">The username of the user to subscribe to.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The details of the newly created or reactivated subscription.</returns>
        /// <response code="200">Returns the subscription details.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the user to subscribe to is not found.</response>
        /// <response code="409">If the user is already subscribed.</response>
        [HttpPost("{username}/subscribe")]
        [Authorize]
        [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<SubscriptionDto>> Subscribe(string username, CancellationToken cancellationToken)
        {
            var subscriberIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(subscriberIdString))
            {
                return Unauthorized();
            }
            var subscriberId = int.Parse(subscriberIdString);
            var userToSubscribe = await _userService.GetByUsernameAsync(username, cancellationToken);
            if (userToSubscribe is null)
            {
                return NotFound($"User with username '{username}' not found.");
            }
            var createDto = new SubscriptionCreateDto { SubscribedToId = userToSubscribe.Id };
            var subscriptionDto = await _subscriptionService.SubscribeAsync(subscriberId, createDto, cancellationToken);
            return Ok(subscriptionDto);
        }

        /// <summary>
        /// Unsubscribes the currently authenticated user from another user.
        /// </summary>
        /// <param name="username">The username of the user to unsubscribe from.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the action was successful.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the user to unsubscribe from is not found.</response>
        [HttpDelete("{username}/subscribe")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Unsubscribe(string username, CancellationToken cancellationToken)
        {
            var subscriberIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(subscriberIdString))
            {
                return Unauthorized();
            }
            var subscriberId = int.Parse(subscriberIdString);
            var userToUnsubscribe = await _userService.GetByUsernameAsync(username, cancellationToken);
            if (userToUnsubscribe is null)
            {
                return NotFound($"User with username '{username}' not found.");
            }
            await _subscriptionService.UnsubscribeAsync(subscriberId, userToUnsubscribe.Id, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Retrieves a list of users that a specific user is subscribed to (their "following" list).
        /// </summary>
        /// <param name="username">The username of the user whose "following" list is to be retrieved.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of subscription details.</returns>
        /// <response code="200">Returns the list of subscriptions.</response>
        /// <response code="404">If the user with the specified username is not found.</response>
        [HttpGet("{username}/subscriptions")]
        [ProducesResponseType(typeof(IReadOnlyList<SubscriptionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<SubscriptionDto>>> GetSubscriptions(string username, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByUsernameAsync(username, cancellationToken);
            if (user is null)
            {
                return NotFound($"User with username '{username}' not found.");
            }
            var subscriptions = await _subscriptionService.GetSubscriptionsAsync(user.Id, cancellationToken);
            return Ok(subscriptions);
        }

        /// <summary>
        /// Retrieves a list of users who are subscribed to a specific user (their "followers" list).
        /// </summary>
        /// <param name="username">The username of the user whose "followers" list is to be retrieved.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of subscription details representing the followers.</returns>
        /// <response code="200">Returns the list of subscribers.</response>
        /// <response code="404">If the user with the specified username is not found.</response>
        [HttpGet("{username}/subscribers")]
        [ProducesResponseType(typeof(IReadOnlyList<SubscriptionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<SubscriptionDto>>> GetSubscribers(string username, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByUsernameAsync(username, cancellationToken);
            if (user is null)
            {
                return NotFound($"User with username '{username}' not found.");
            }
            var subscribers = await _subscriptionService.GetSubscribersAsync(user.Id, cancellationToken);
            return Ok(subscribers);
        }
    }
}