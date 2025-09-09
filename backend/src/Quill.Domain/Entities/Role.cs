using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Domain.Entities
{
    public class Role : BaseEntity
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public string Name { get; set; } // Admin, Author
        public ICollection<User> Users { get; set; }
    }
}