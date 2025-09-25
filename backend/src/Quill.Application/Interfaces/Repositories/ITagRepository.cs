using Quill.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Quill.Application.Interfaces.Repositories
{
    /// <summary>
    /// Defines the contract for data operations on the Tag entity.
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        /// Retrieves a read-only list of all tags from the database.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a read-only list of all tags.</returns>
        Task<IReadOnlyList<Tag>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single tag by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tag.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the tag if found; otherwise, null.</returns>
        Task<Tag?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single tag by its unique name.
        /// </summary>
        /// <param name="name">The unique name of the tag.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the tag if found; otherwise, null.</returns>
        Task<Tag?> GetByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Marks a new tag to be added to the database.
        /// </summary>
        /// <param name="tag">The new tag entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(Tag tag, CancellationToken cancellationToken);

        /// <summary>
        /// Marks an existing tag as modified in the change tracker.
        /// </summary>
        /// <param name="tag">The tag entity to update.</param>
        void Update(Tag tag);

        /// <summary>
        /// Marks an existing tag for deletion from the database.
        /// </summary>
        /// <param name="tag">The tag entity to remove.</param>
        void Remove(Tag tag);
    }
}