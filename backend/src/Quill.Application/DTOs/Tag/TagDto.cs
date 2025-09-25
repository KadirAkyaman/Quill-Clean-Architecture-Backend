namespace Quill.Application.DTOs.Tag
{
    /// <summary>
    /// Represents a tag as returned by the API.
    /// </summary>
    public class TagDto
    {
        /// <summary>
        /// The unique identifier for the tag.
        /// </summary>
        /// <example>8</example>
        public int Id { get; set; }

        /// <summary>
        /// The name of the tag.
        /// </summary>
        /// <example>Clean Architecture</example>
        public string Name { get; set; }

        /// <summary>
        
        /// The number of posts associated with this tag.
        /// </summary>
        /// <example>15</example>
        public int PostCount { get; set; }
    }
}