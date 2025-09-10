using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Domain.Enums;

namespace Quill.Application.DTOs.Post
{
    public class PostUpdateDto
    {
        public PostUpdateDto()
        {
            TagIds = new HashSet<int>();
        }
        
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Summary { get; set; }
        public int? CategoryId { get; set; }
        public ICollection<int>? TagIds { get; set; }
        public PostStatus? Status { get; set; }
    }
}