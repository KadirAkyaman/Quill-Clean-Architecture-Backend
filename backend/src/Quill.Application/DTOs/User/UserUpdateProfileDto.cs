using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Application.DTOs.User
{
    public class UserUpdateProfileDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Username { get; set; }
        public string? ProfilePictureURL { get; set; } //The user does not fill in the ProfilePictureURL field. Our Controller fills it in, processes the file, and saves it to a secure location.
    }
}