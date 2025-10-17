using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Quill.Application.DTOs.Post;
using Quill.Application.Interfaces.Services;

namespace Quill.Presentation.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableRateLimiting("fixed")]
    [Tags("Posts")]
    [ApiVersion("1.0")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Retrieves a preview list of all posts.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of post previews.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<PostPreviewDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PostPreviewDto>>> GetAll(CancellationToken cancellationToken)
        {
            var posts = await _postService.GetAllAsync(cancellationToken);
            return Ok(posts);
        }

        /// <summary>
        /// Retrieves a single, detailed post by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the post.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The full details of the post.</returns>
        /// <response code="200">Returns the requested post.</response>
        /// <response code="404">If a post with the specified ID is not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var post = await _postService.GetByIdAsync(id, cancellationToken);
            if (post is null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        /// <summary>
        /// Retrieves a preview list of all posts by a specific author.
        /// </summary>
        /// <param name="authorId">The unique identifier of the author.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of the author's posts.</returns>
        [HttpGet("author/{authorId:int}")]
        [ProducesResponseType(typeof(IReadOnlyList<PostPreviewDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PostPreviewDto>>> GetByAuthorId(int authorId, CancellationToken cancellationToken)
        {
            var authorPosts = await _postService.GetByAuthorIdAsync(authorId, cancellationToken);
            return Ok(authorPosts);
        }

        /// <summary>
        /// Retrieves a preview list of all posts in a specific category by its ID.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of posts in the specified category.</returns>
        [HttpGet("category/{categoryId:int}")]
        [ProducesResponseType(typeof(IReadOnlyList<PostPreviewDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PostPreviewDto>>> GetByCategoryId(int categoryId, CancellationToken cancellationToken)
        {
            var categoryPosts = await _postService.GetByCategoryIdAsync(categoryId, cancellationToken);
            return Ok(categoryPosts);
        }

        /// <summary>
        /// Retrieves a preview list of all posts in a specific category by its name.
        /// </summary>
        /// <param name="categoryName">The unique name of the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of posts in the specified category.</returns>
        [HttpGet("category/name/{categoryName}")]
        [ProducesResponseType(typeof(IReadOnlyList<PostPreviewDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PostPreviewDto>>> GetByCategoryName(string categoryName, CancellationToken cancellationToken)
        {
            var categoryPosts = await _postService.GetByCategoryNameAsync(categoryName, cancellationToken);
            return Ok(categoryPosts);
        }

        /// <summary>
        /// Retrieves a preview list of all posts associated with a specific tag.
        /// </summary>
        /// <param name="tagName">The name of the tag.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of posts associated with the specified tag.</returns>
        [HttpGet("tag/{tagName}")]
        [ProducesResponseType(typeof(IReadOnlyList<PostPreviewDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PostPreviewDto>>> GetByTagName(string tagName, CancellationToken cancellationToken)
        {
            var tagPosts = await _postService.GetByTagNameAsync(tagName, cancellationToken);
            return Ok(tagPosts);
        }

        /// <summary>
        /// Retrieves a specified number of the most recent posts.
        /// </summary>
        /// <param name="count">The number of recent posts to retrieve.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of the most recent posts.</returns>
        [HttpGet("recent/{count:int}")]
        [ProducesResponseType(typeof(IReadOnlyList<PostPreviewDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PostPreviewDto>>> GetRecent(int count, CancellationToken cancellationToken)
        {
            var recentPosts = await _postService.GetRecentAsync(count, cancellationToken);
            return Ok(recentPosts);
        }

        /// <summary>
        /// Creates a new post. (Authentication required)
        /// </summary>
        /// <param name="postCreateDto">The data needed to create the post.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The newly created post.</returns>
        /// <response code="201">Returns the newly created post.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PostDto>> Create([FromBody] PostCreateDto postCreateDto, CancellationToken cancellationToken)
        {
            var authorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(authorIdString))
            {
                return Unauthorized("User ID claim not found in token.");
            }
            var authorId = int.Parse(authorIdString);

            var createdPost = await _postService.CreateAsync(authorId, postCreateDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = createdPost.Id }, createdPost);
        }

        /// <summary>
        /// Updates an existing post. (Authentication required; user must be the author)
        /// </summary>
        /// <param name="postId">The unique identifier of the post to update.</param>
        /// <param name="postUpdateDto">The new data for the post.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the update was successful.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not the author of the post.</response>
        /// <response code="404">If the post to update is not found.</response>
        [HttpPut("{postId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int postId, [FromBody] PostUpdateDto postUpdateDto, CancellationToken cancellationToken)
        {
            var authorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(authorIdString))
            {
                return Unauthorized("User ID claim not found in token.");
            }
            var authorId = int.Parse(authorIdString);

            await _postService.UpdateAsync(postId, authorId, postUpdateDto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes a post. (Authentication required; user must be the author)
        /// </summary>
        /// <param name="postId">The unique identifier of the post to delete.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">If the deletion was successful.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not the author of the post.</response>
        /// <response code="404">If the post to delete is not found.</response>
        [HttpDelete("{postId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int postId, CancellationToken cancellationToken)
        {
            var authorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(authorIdString))
            {
                return Unauthorized("User ID claim not found in token.");
            }
            var authorId = int.Parse(authorIdString);

            await _postService.DeleteAsync(postId, authorId, cancellationToken);
            return NoContent();
        }
    }
}