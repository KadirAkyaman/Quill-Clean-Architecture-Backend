using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Quill.Application.DTOs.User;
using Quill.Application.Interfaces.Services;

namespace Quill.Presentation.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableRateLimiting("auth")]
    [Tags("Authentication")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        // ../logout 
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="userRegisterDto">The registration details for the new user.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An authentication response containing a JWT upon successful registration.</returns>
        /// <response code="200">Returns the authentication response with a new JWT.</response>
        /// <response code="400">If the provided registration data is invalid.</response>
        /// <response code="409">If the email or username is already in use.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] UserRegisterDto userRegisterDto, CancellationToken cancellationToken)
        {
            var response = await _userService.RegisterAsync(userRegisterDto, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Authenticates a user and provides a JWT for subsequent requests.
        /// </summary>
        /// <param name="userLoginDto">The login credentials of the user.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An authentication response containing a JWT upon successful login.</returns>
        /// <response code="200">Returns the authentication response with a new JWT.</response>
        /// <response code="401">If the provided credentials are invalid.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] UserLoginDto userLoginDto, CancellationToken cancellationToken)
        {
            var response = await _userService.LoginAsync(userLoginDto, cancellationToken);
            return Ok(response);
        }
    }
}