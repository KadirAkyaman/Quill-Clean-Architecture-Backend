using Quill.Application.DTOs.Category;
using Quill.Application.DTOs.Tag;
using Quill.Application.DTOs.User;

namespace Quill.Application.DTOs.Post
{
    /// <summary>
    /// Represents the detailed view of a single post, including its full content.
    /// </summary>
    public class PostDto
    {
        public PostDto()
        {
            Tags = new HashSet<TagDto>();
        }

        /// <summary>
        /// The unique identifier for the post.
        /// </summary>
        /// <example>123</example>
        public int Id { get; set; }

        /// <summary>
        /// The title of the post.
        /// </summary>
        /// <example>A Deep Dive into Clean Architecture</example>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The full content of the post.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// A short summary of the post.
        /// </summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// The date and time the post was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date and time the post was last updated, if applicable.
        /// </summary>
        public DateTime? LastUpdatedAt { get; set; }

        /// <summary>
        /// The current status of the post (e.g., "Draft", "Published").
        /// </summary>
        /// <example>Published</example>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// A summary of the author who wrote the post.
        /// </summary>
        public UserSummaryDto Author { get; set; } = null!;

        /// <summary>
        /// The category the post belongs to.
        /// </summary>
        public CategoryDto Category { get; set; } = null!;

        /// <summary>
        /// A collection of tags associated with the post.
        /// </summary>
        public ICollection<TagDto> Tags { get; set; }
    }
}