using Quill.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Quill.Application.Interfaces.Repositories
{
    /// <summary>
    /// Defines the contract for data operations on the Post entity.
    /// </summary>
    public interface IPostRepository
    {
        /// <summary>
        /// Retrieves a read-only list of all posts, including their author and category.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of posts.</returns>
        Task<IReadOnlyList<Post>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single post by its unique identifier, including all related data (author, category, tags).
        /// </summary>
        /// <param name="postId">The unique identifier of the post.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the post if found; otherwise, null.</returns>
        Task<Post?> GetByIdAsync(int postId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all posts written by a specific author.
        /// </summary>
        /// <param name="authorId">The unique identifier of the author.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of the author's posts.</returns>
        Task<IReadOnlyList<Post>> GetByAuthorIdAsync(int authorId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all posts belonging to a specific category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of posts in the category.</returns>
        Task<IReadOnlyList<Post>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Retrieves all posts belonging to a specific category by its name.
        /// </summary>
        /// <param name="categoryName">The unique name of the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of posts in the category.</returns>
        Task<IReadOnlyList<Post>> GetByCategoryNameAsync(string categoryName, CancellationToken cancellationToken);
        
        /// <summary>
        /// Retrieves all posts associated with a specific tag name.
        /// </summary>
        /// <param name="tagName">The name of the tag.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of posts with the specified tag.</returns>
        Task<IReadOnlyList<Post>> GetByTagNameAsync(string tagName, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a specified number of the most recent posts.
        /// </summary>
        /// <param name="count">The number of recent posts to retrieve.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of the most recent posts.</returns>
        Task<IReadOnlyList<Post>> GetRecentAsync(int count, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the total number of posts written by a specific author.
        /// </summary>
        /// <param name="authorId">The unique identifier of the author.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total number of posts.</returns>
        Task<int> GetCountByAuthorIdAsync(int authorId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a specified number of the most recent posts by a specific author.
        /// </summary>
        /// <param name="authorId">The unique identifier of the author.</param>
        /// <param name="count">The number of recent posts to retrieve.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of the author's most recent posts.</returns>
        Task<IReadOnlyList<Post>> GetRecentByAuthorIdAsync(int authorId, int count, CancellationToken cancellationToken);
        
        /// <summary>
        /// Marks a new post to be added to the database.
        /// </summary>
        /// <param name="post">The new post entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(Post post, CancellationToken cancellationToken);

        /// <summary>
        /// Marks an existing post as modified in the change tracker.
        /// </summary>
        /// <param name="post">The post entity to update.</param>
        void Update(Post post);
        
        /// <summary>
        /// Marks an existing post for deletion from the database.
        /// </summary>
        /// <param name="post">The post entity to remove.</param>
        void Remove(Post post);
    }
}