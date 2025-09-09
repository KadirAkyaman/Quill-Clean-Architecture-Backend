using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public Tag()
        {
            Posts = new HashSet<PostTag>();
        }
        
        public string Name { get; set; }
        public ICollection<PostTag> Posts { get; set; }
    }
}