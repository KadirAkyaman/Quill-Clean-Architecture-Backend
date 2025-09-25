using System.ComponentModel.DataAnnotations;

namespace Quill.Application.DTOs.Category
{
    /// <summary>
    /// Represents the data required to update an existing category.
    /// Only non-null properties will be updated.
    /// </summary>
    public class CategoryUpdateDto
    {
        /// <summary>
        /// The new name for the category. Must be unique if provided.
        /// </summary>
        /// <example>Software Development</example>
        public string? Name { get; set; }
    }
}