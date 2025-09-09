using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Posts = new HashSet<Post>();
        }
        
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}