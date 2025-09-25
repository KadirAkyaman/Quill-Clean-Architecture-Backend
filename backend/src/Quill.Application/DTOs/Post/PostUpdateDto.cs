using Quill.Domain.Enums;

namespace Quill.Application.DTOs.Post
{
    /// <summary>
    /// Represents the data required to update an existing post. 
    /// Only non-null properties will be updated.
    /// </summary>
    public class PostUpdateDto
    {
        public PostUpdateDto()
        {
            TagIds = new HashSet<int>();
        }

        /// <summary>
        /// The new title for the post. If provided, must be between 5 and 200 characters.
        /// </summary>
        /// <example>A Deep Dive into Clean Architecture</example>
        public string? Title { get; set; }

        /// <summary>
        /// The new main content for the post. If provided, must be at least 50 characters long.
        /// </summary>
        /// <example>Expanding on the core principles, this article explores advanced topics in Clean Architecture...</example>
        public string? Text { get; set; }

        /// <summary>
        /// The new summary for the post. If provided, must be between 10 and 300 characters.
        /// </summary>
        /// <example>An in-depth look at the advanced topics and practical implementation of Clean Architecture.</example>
        public string? Summary { get; set; }

        /// <summary>
        /// The new unique identifier for the post's category.
        /// </summary>
        /// <example>6</example>
        public int? CategoryId { get; set; }

        /// <summary>
        /// The new collection of unique identifiers for the post's tags.
        /// </summary>
        /// <example>[1, 8, 15]</example>
        public ICollection<int>? TagIds { get; set; }

        /// <summary>
        /// The new status for the post (e.g., Draft, Published).
        /// </summary>
        /// <example>Published</example>
        public PostStatus? Status { get; set; }
    }
}