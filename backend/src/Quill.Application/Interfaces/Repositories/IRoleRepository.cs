using Quill.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Quill.Application.Interfaces.Repositories
{
    /// <summary>
    /// Defines the contract for data operations on the Role entity.
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Retrieves a read-only list of all roles from the database.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a read-only list of all roles.</returns>
        Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single role by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the role.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the role if found; otherwise, null.</returns>
        Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single role by its unique name.
        /// </summary>
        /// <param name="name">The unique name of the role.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the role if found; otherwise, null.</returns>
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Marks a new role to be added to the database.
        /// </summary>
        /// <param name="role">The new role entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(Role role, CancellationToken cancellationToken);

        /// <summary>
        /// Marks an existing role as modified in the change tracker.
        /// </summary>
        /// <param name="role">The role entity to update.</param>
        void Update(Role role);

        /// <summary>
        /// Marks an existing role for deletion from the database.
        /// </summary>
        /// <param name="role">The role entity to remove.</param>
        void Remove(Role role);
    }
}