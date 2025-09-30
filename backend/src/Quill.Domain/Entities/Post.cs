using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Domain.Enums;

namespace Quill.Domain.Entities
{
    public class Post : BaseEntity
    {
        public Post()
        {
            Tags = new HashSet<PostTag>();
        }

        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string Summary { get; set; }  = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<PostTag> Tags { get; set; }
        public PostStatus Status { get; set; } = PostStatus.Draft;
    }
}
