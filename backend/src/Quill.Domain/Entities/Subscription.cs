using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Domain.Entities
{
    public class Subscription : BaseEntity
    {
        public int SubscriberId { get; set; }
        public User Subscriber { get; set; } = null!;
        public int SubscribedToId { get; set; }
        public User SubscribedTo { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}