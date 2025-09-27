using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quill.Application.DTOs.Tag;
using Quill.Application.Interfaces.Services;

namespace Quill.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Retrieves a list of all tags.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of all available tags.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<TagDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<TagDto>>> GetAll(CancellationToken cancellationToken)
        {
            var tags = await _tagService.GetAllAsync(cancellationToken);
            return Ok(tags);
        }

        /// <summary>
        /// Retrieves a single tag by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tag.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The tag with the specified ID.</returns>
        /// <response code="200">Returns the requested tag.</response>
        /// <response code="404">If a tag with the specified ID is not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)] // DÜZELTİLDİ
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TagDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var tag = await _tagService.GetByIdAsync(id, cancellationToken);
            if (tag is null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        /// <summary>
        /// Retrieves a single tag by its unique name.
        /// </summary>
        /// <param name="name">The unique name of the tag.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The tag with the specified name.</returns>
        /// <response code="200">Returns the requested tag.</response>
        /// <response code="404">If a tag with the specified name is not found.</response>
        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)] // DÜZELTİLDİ
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TagDto>> GetByName(string name, CancellationToken cancellationToken)
        {
            var tag = await _tagService.GetByNameAsync(name, cancellationToken);
            if (tag is null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        /// <summary>
        /// Creates a new tag. (Admin Only)
        /// </summary>
        /// <param name="tagCreateDto">The data needed to create the tag.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The newly created tag.</returns>
        /// <response code="201">Returns the newly created tag.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="409">If a tag with the same name already exists.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(TagDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<TagDto>> Create([FromBody] TagCreateDto tagCreateDto, CancellationToken cancellationToken)
        {
            var createdTag = await _tagService.CreateAsync(tagCreateDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = createdTag.Id }, createdTag);
        }

        /// <summary>
        /// Updates an existing tag. (Admin Only)
        /// </summary>
        /// <param name="id">The unique identifier of the tag to update.</param>
        /// <param name="tagUpdateDto">The new data for the tag.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the update was successful.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="404">If the tag to update is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(int id, [FromBody] TagUpdateDto tagUpdateDto, CancellationToken cancellationToken)
        {
            await _tagService.UpdateAsync(id, tagUpdateDto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes a tag. (Admin Only)
        /// </summary>
        /// <param name="id">The unique identifier of the tag to delete.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the deletion was successful.</response>
        /// <response code="404">If the tag to delete is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _tagService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }       
    }
}