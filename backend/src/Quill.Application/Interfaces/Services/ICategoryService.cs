using Quill.Application.DTOs.Category;
using Quill.Application.Exceptions;

namespace Quill.Application.Interfaces.Services
{
    /// <summary>
    /// Defines business logic operations for Category entities.
    /// This service handles the creation, retrieval, updating, and deletion of categories.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves a list of all categories available in the system.
        /// </summary>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of Category DTOs.</returns>
        Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the found Category DTO, or null if not found.</returns>
        Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single category by its unique name.
        /// </summary>
        /// <param name="name">The unique name of the category.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the found Category DTO, or null if not found.</returns>
        Task<CategoryDto?> GetByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new category based on the provided data.
        /// </summary>
        /// <param name="createDto">The DTO containing the data for the new category.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the DTO of the newly created category, including its server-generated ID.</returns>
        /// <exception cref="ConflictException">Thrown if a category with the same name already exists.</exception>
        Task<CategoryDto> CreateAsync(CategoryCreateDto createDto, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing category's information.
        /// </summary>
        /// <param name="id">The unique identifier of the category to update.</param>
        /// <param name="updateDto">The DTO containing the new data for the category.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the category with the specified ID is not found.</exception>
        /// <exception cref="ConflictException">Thrown if the new category name conflicts with another existing category.</exception>
        Task UpdateAsync(int id, CategoryUpdateDto updateDto, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a category from the system by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the category with the specified ID is not found.</exception>
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}