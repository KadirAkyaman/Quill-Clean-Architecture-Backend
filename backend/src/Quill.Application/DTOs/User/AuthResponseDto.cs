using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Application.DTOs.User
{
    public class AuthResponseDto //  This is the standard model that your login endpoint will return upon success.
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}