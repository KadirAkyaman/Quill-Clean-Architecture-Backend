using System.ComponentModel.DataAnnotations;

namespace Quill.Application.DTOs.Category
{
    /// <summary>
    /// Represents the data required to create a new category.
    /// </summary>
    public class CategoryCreateDto
    {
        /// <summary>
        /// The name of the new category. Must be unique.
        /// </summary>
        /// <example>Technology</example>
        public string Name { get; set; } = string.Empty;
    }
}