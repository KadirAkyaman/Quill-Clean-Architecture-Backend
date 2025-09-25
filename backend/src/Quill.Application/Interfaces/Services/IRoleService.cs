using Quill.Application.DTOs.Role;
using Quill.Application.Exceptions;

namespace Quill.Application.Interfaces.Services
{
    /// <summary>
    /// Defines business logic operations for Role entities.
    /// This service is typically restricted to administrators.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Retrieves a list of all roles in the system.
        /// </summary>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of Role DTOs.</returns>
        Task<IReadOnlyList<RoleDto>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single role by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the role.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the found Role DTO, or null if not found.</returns>
        Task<RoleDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single role by its unique name.
        /// </summary>
        /// <param name="name">The unique name of the role.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the found Role DTO, or null if not found.</returns>
        Task<RoleDto?> GetByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="roleCreateDto">The DTO containing the data for the new role.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the DTO of the newly created role.</returns>
        /// <exception cref="ConflictException">Thrown if a role with the same name already exists.</exception>
        Task<RoleDto> CreateAsync(RoleCreateDto roleCreateDto, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing role's name.
        /// </summary>
        /// <param name="id">The unique identifier of the role to update.</param>
        /// <param name="roleUpdateDto">The DTO containing the new data for the role.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the role with the specified ID is not found.</exception>
        /// <exception cref="ConflictException">Thrown if the new role name conflicts with an existing one.</exception>
        Task UpdateAsync(int id, RoleUpdateDto roleUpdateDto, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a role by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the role to delete.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the role with the specified ID is not found.</exception>
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}