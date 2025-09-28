namespace Quill.Application.DTOs.Post
{
    /// <summary>
    /// Represents the data required to create a new post.
    /// </summary>
    public class PostCreateDto
    {
        public PostCreateDto()
        {
            TagIds = new HashSet<int>();
        }

        /// <summary>
        /// The title of the post. Must be between 5 and 200 characters.
        /// </summary>
        /// <example>Introduction to Clean Architecture</example>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The main content of the post in rich text or markdown format. Must be at least 50 characters long.
        /// </summary>
        /// <example>Clean Architecture is a software design philosophy that separates the elements of a design into ring levels...</example>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// A short summary of the post. Must be between 10 and 300 characters.
        /// </summary>
        /// <example>A brief overview of the core principles of Clean Architecture.</example>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// The unique identifier of the category this post belongs to.
        /// </summary>
        /// <example>5</example>
        public int CategoryId { get; set; }

        /// <summary>
        /// A collection of unique identifiers for the tags associated with this post.
        /// </summary>
        /// <example>[1, 8, 12]</example>
        public ICollection<int> TagIds { get; set; }
    }
}