using Quill.Application.DTOs.Tag;
using Quill.Application.Exceptions;

namespace Quill.Application.Interfaces.Services
{
    /// <summary>
    /// Defines business logic operations for Tag entities.
    /// This service handles the creation, retrieval, updating, and deletion of tags.
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// Retrieves a list of all tags available in the system.
        /// </summary>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of Tag DTOs.</returns>
        Task<IReadOnlyList<TagDto>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single tag by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tag.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the found Tag DTO, or null if not found.</returns>
        Task<TagDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single tag by its unique name.
        /// </summary>
        /// <param name="name">The unique name of the tag.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the found Tag DTO, or null if not found.</returns>
        Task<TagDto?> GetByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="tagCreateDto">The DTO containing the data for the new tag.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the DTO of the newly created tag.</returns>
        /// <exception cref="ConflictException">Thrown if a tag with the same name already exists.</exception>
        Task<TagDto> CreateAsync(TagCreateDto tagCreateDto, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing tag's name.
        /// </summary>
        /// <param name="id">The unique identifier of the tag to update.</param>
        /// <param name="tagUpdateDto">The DTO containing the new data for the tag.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the tag with the specified ID is not found.</exception>
        /// <exception cref="ConflictException">Thrown if the new tag name conflicts with an existing one.</exception>
        Task UpdateAsync(int id, TagUpdateDto tagUpdateDto, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a tag by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tag to delete.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the tag with the specified ID is not found.</exception>
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}