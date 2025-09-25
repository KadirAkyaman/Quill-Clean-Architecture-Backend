using Quill.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Quill.Application.Interfaces.Repositories
{
    /// <summary>
    /// Defines the contract for data operations on the Category entity.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Retrieves a read-only list of all categories from the database.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a read-only list of all categories.</returns>
        Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the category if found; otherwise, null.</returns>
        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single category by its unique name.
        /// </summary>
        /// <param name="name">The unique name of the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the category if found; otherwise, null.</returns>
        Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Marks a new category to be added to the database.
        /// </summary>
        /// <param name="category">The new category entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(Category category, CancellationToken cancellationToken);

        /// <summary>
        /// Marks an existing category as modified in the change tracker.
        /// </summary>
        /// <param name="category">The category entity to update.</param>
        void Update(Category category);

        /// <summary>
        /// Marks an existing category for deletion from the database.
        /// </summary>
        /// <param name="category">The category entity to remove.</param>
        void Remove(Category category);
    }
}