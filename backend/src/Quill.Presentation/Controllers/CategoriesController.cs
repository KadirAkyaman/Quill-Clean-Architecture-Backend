using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Quill.Application.DTOs.Category;
using Quill.Application.Interfaces.Services;

namespace Quill.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("fixed")]
    [Tags("Categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //                                                                              --- PUBLIC ENDPOINTS

        /// <summary>
        /// Retrieves a list of all categories.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of all available categories.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);

            return Ok(categories);
        }

        /// <summary>
        /// Retrieves a single category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The category with the specified ID.</returns>
        /// <response code="200">Returns the requested category.</response>
        /// <response code="404">If a category with the specified ID is not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(id, cancellationToken);

            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Retrieves a single category by its unique name.
        /// </summary>
        /// <param name="name">The unique name of the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The category with the specified name.</returns>
        /// <response code="200">Returns the requested category.</response>
        /// <response code="404">If a category with the specified name is not found.</response>
        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> GetByName(string name, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByNameAsync(name, cancellationToken);

            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        //                                                                              --- ADMIN ENDPOINTS

        /// <summary>
        /// Creates a new category. (Admin Only)
        /// </summary>
        /// <param name="createDto">The data needed to create the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The newly created category.</returns>
        /// <response code="201">Returns the newly created category.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="409">If a category with the same name already exists.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            var createdCategory = await _categoryService.CreateAsync(createDto, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }

        /// <summary>
        /// Updates an existing category. (Admin Only)
        /// </summary>
        /// <param name="id">The unique identifier of the category to update.</param>
        /// <param name="categoryUpdateDto">The new data for the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the update was successful.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="404">If the category to update is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an administrator.</response>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken)
        {
            await _categoryService.UpdateAsync(id, categoryUpdateDto, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Deletes a category. (Admin Only)
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the deletion was successful.</response>
        /// <response code="404">If the category to delete is not found.</response>
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
            await _categoryService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}