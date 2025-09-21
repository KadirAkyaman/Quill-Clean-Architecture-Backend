using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Domain.Entities;

namespace Quill.Application.Interfaces.Services
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        string GenerateJwtToken(User user);
    }
}