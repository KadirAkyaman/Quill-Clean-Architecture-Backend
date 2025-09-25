using Quill.Application.DTOs.Post;
using Quill.Application.Exceptions;

namespace Quill.Application.Interfaces.Services
{
    /// <summary>
    /// Defines business logic operations for Post entities.
    /// This service handles the creation, retrieval, updating, and deletion of posts, enforcing business rules and authorization.
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Retrieves a preview list of all posts, suitable for general listing pages.
        /// </summary>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of Post Preview DTOs.</returns>
        Task<IReadOnlyList<PostPreviewDto>> GetAllAsync(CancellationToken cancellationToken);
        
        /// <summary>
        /// Retrieves a single, detailed post by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the post.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the full Post DTO, or null if not found.</returns>
        Task<PostDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        
        /// <summary>
        /// Retrieves a preview list of all posts written by a specific author.
        /// </summary>
        /// <param name="authorId">The unique identifier of the author.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of the author's posts.</returns>
        Task<IReadOnlyList<PostPreviewDto>> GetByAuthorIdAsync(int authorId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Retrieves a preview list of all posts belonging to a specific category, identified by its ID.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of posts in the category.</returns>
        Task<IReadOnlyList<PostPreviewDto>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Retrieves a preview list of all posts belonging to a specific category, identified by its name.
        /// </summary>
        /// <param name="categoryName">The unique name of the category.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of posts in the category.</returns>
        Task<IReadOnlyList<PostPreviewDto>> GetByCategoryNameAsync(string categoryName, CancellationToken cancellationToken);
        
        /// <summary>
        /// Retrieves a preview list of all posts associated with a specific tag name.
        /// </summary>
        /// <param name="tagName">The name of the tag.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of posts with the specified tag.</returns>
        Task<IReadOnlyList<PostPreviewDto>> GetByTagNameAsync(string tagName, CancellationToken cancellationToken);
        
        /// <summary>
        /// Retrieves a specified number of the most recent posts, typically for a homepage feed.
        /// </summary>
        /// <param name="count">The number of recent posts to retrieve.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a read-only list of the most recent posts.</returns>
        Task<IReadOnlyList<PostPreviewDto>> GetRecentAsync(int count, CancellationToken cancellationToken);
        
        /// <summary>
        /// Creates a new post on behalf of the authenticated user.
        /// </summary>
        /// <param name="authorId">The ID of the authenticated user (from JWT).</param>
        /// <param name="postCreateDto">The data for the new post.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the full DTO of the newly created post.</returns>
        /// <exception cref="NotFoundException">Thrown if the CategoryId provided in the DTO is invalid.</exception>
        Task<PostDto> CreateAsync(int authorId, PostCreateDto postCreateDto, CancellationToken cancellationToken);
        
        /// <summary>
        /// Updates an existing post, ensuring the action is performed by the original author.
        /// </summary>
        /// <param name="postId">The ID of the post to update.</param>
        /// <param name="authorId">The ID of the authenticated user attempting the update.</param>
        /// <param name="postUpdateDto">The DTO containing the new data for the post.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the post with the specified ID is not found.</exception>
        /// <exception cref="UnauthorizedActionException">Thrown if the user is not the author of the post.</exception>
        Task UpdateAsync(int postId, int authorId, PostUpdateDto postUpdateDto, CancellationToken cancellationToken);
        
        /// <summary>
        /// Deletes a post, ensuring the action is performed by the original author.
        /// </summary>
        /// <param name="postId">The ID of the post to delete.</param>
        /// <param name="authorId">The ID of the authenticated user attempting the deletion.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown if the post with the specified ID is not found.</exception>
        /// <exception cref="UnauthorizedActionException">Thrown if the user is not the author of the post.</exception>
        Task DeleteAsync(int postId, int authorId, CancellationToken cancellationToken);
    }
}