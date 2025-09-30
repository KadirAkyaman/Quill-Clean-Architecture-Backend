using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Domain.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Posts = new HashSet<Post>();
            Subscribers = new HashSet<Subscription>();
            Subscriptions = new HashSet<Subscription>();
        }
        
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!; 
        public string Username { get; set; } = null!; 
        public string PasswordHash { get; set; } = null!;
        public string? ProfilePictureURL { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Subscription> Subscribers { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; } 
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}