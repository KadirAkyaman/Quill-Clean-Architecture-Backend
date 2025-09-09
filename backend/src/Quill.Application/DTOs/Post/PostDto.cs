using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Application.DTOs.Category;
using Quill.Application.DTOs.Tag;
using Quill.Application.DTOs.User;

namespace Quill.Application.DTOs.Post
{
    public class PostDto
    {
        public PostDto()
        {
            Tags = new HashSet<TagDto>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Summary { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public UserSummaryDto Author { get; set; }
        public CategoryDto Category { get; set; }
        public ICollection<TagDto> Tags { get; set; }
    }
}