using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Application.DTOs.User
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}