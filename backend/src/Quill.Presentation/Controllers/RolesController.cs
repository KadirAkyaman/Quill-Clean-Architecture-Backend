using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quill.Application.DTOs.Role;
using Quill.Application.Interfaces.Services;

namespace Quill.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Roles")]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Retrieves a list of all roles.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of all available roles.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<RoleDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetAll(CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetAllAsync(cancellationToken);
            return Ok(roles);

        }

        /// <summary>
        /// Retrieves a single role by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the role.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The role with the specified ID.</returns>
        /// <response code="200">Returns the requested role.</response>
        /// <response code="404">If a role with the specified ID is not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoleDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetByIdAsync(id, cancellationToken);
            if (role is null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        /// <summary>
        /// Retrieves a single role by its unique name.
        /// </summary>
        /// <param name="name">The unique name of the role.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The role with the specified name.</returns>
        /// <response code="200">Returns the requested role.</response>
        /// <response code="404">If a role with the specified name is not found.</response>
        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoleDto>> GetByName(string name, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetByNameAsync(name, cancellationToken);
            if (role is null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        /// <summary>
        /// Creates a new role. (Admin Only)
        /// </summary>
        /// <param name="roleCreateDto">The data needed to create the role.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The newly created role.</returns>
        /// <response code="201">Returns the newly created role.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="409">If a role with the same name already exists.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpPost]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RoleDto>> Create([FromBody] RoleCreateDto roleCreateDto, CancellationToken cancellationToken)
        {
            var createdRole = await _roleService.CreateAsync(roleCreateDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = createdRole.Id }, createdRole);
        }

        /// <summary>
        /// Updates an existing role. (Admin Only)
        /// </summary>
        /// <param name="id">The unique identifier of the role to update.</param>
        /// <param name="roleUpdateDto">The new data for the role.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the update was successful.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="404">If the role to update is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(int id, [FromBody] RoleUpdateDto roleUpdateDto, CancellationToken cancellationToken)
        {
            await _roleService.UpdateAsync(id, roleUpdateDto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes a role. (Admin Only)
        /// </summary>
        /// <param name="id">The unique identifier of the role to delete.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the deletion was successful.</response>
        /// <response code="404">If the role to delete is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _roleService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}