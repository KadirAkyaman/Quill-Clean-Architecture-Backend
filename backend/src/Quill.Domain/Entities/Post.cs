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
        public User User { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Summary { get; set; } // max 150 char
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<PostTag> Tags { get; set; }
        public PostStatus Status { get; set; } = PostStatus.Draft;
    }
}
