using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Application.DTOs.Post;

namespace Quill.Application.DTOs.User
{
    public class UserProfileDto // profiles of others
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string ProfilePictureURL { get; set; }
        public DateTime MemberSince { get; set; }
        public string Role { get; set; }
        public UserStatsDto Stats { get; set; }
        public ICollection<PostPreviewDto> RecentPosts { get; set; }
    }
}