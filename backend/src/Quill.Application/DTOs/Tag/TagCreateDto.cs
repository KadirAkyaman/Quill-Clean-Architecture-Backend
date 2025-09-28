namespace Quill.Application.DTOs.Tag
{
    /// <summary>
    /// Represents the data required to create a new tag.
    /// </summary>
    public class TagCreateDto
    {
        /// <summary>
        /// The name of the new tag. Must be unique and between 2 and 25 characters.
        /// </summary>
        /// <example>Clean Architecture</example>
        public string Name { get; set; } = string.Empty;
    }
}