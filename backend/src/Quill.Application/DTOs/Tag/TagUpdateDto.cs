namespace Quill.Application.DTOs.Tag
{
    /// <summary>
    /// Represents the data required to update an existing tag.
    /// Only non-null properties will be updated.
    /// </summary>
    public class TagUpdateDto
    {
        /// <summary>
        /// The new name for the tag. If provided, must be unique and between 2 and 25 characters.
        /// </summary>
        /// <example>Dapper ORM</example>
        public string? Name { get; set; }
    }
}