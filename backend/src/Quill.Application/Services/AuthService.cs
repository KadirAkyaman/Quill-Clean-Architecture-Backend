using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quill.Application.Interfaces.Services;
using Quill.Application.Options;
using Quill.Domain.Entities;

namespace Quill.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtOptions _jwtOptions;

        public AuthService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                // The most important claim: Subject. Indicates who the token belongs to.
                // We will read this value in the Controller using User.FindFirstValue(ClaimTypes.NameIdentifier).
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),

                // Jti (JWT ID): An identifier that ensures each token is unique.
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                // Other useful information such as the user name and role.
                // Role information is critical for the [Authorize(Roles=“...”)] attribute to function.
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            // We convert the key from appsettings.json to a byte array.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            // We combine the secret key and the security algorithm to be used (HmacSha256).
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public string HashPassword(string password)
        {
            EnsurePasswordIsNotNullOrWhitespace(password);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            EnsurePasswordIsNotNullOrWhitespace(password);

            if (string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            var isVerified = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            return isVerified;
        }

        // HELPER
        private void EnsurePasswordIsNotNullOrWhitespace(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or whitespace.");
            }
        }
    }
}