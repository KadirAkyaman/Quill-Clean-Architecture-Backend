using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Application.DTOs.Category
{
    /// <summary>
    /// Represents a category as returned by the API.
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// The unique identifier for the category.
        /// </summary>
        /// <example>15</example>
        public int Id { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        /// <example>Technology</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The number of posts associated with this category.
        /// </summary>
        /// <example>42</example>
        public int PostCount { get; set; }
    }
}