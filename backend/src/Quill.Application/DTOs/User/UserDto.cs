using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Application.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ProfilePictureURL { get; set; }
        public string Role { get; set; }
        public DateTime MemberSince { get; set; }
    }
}