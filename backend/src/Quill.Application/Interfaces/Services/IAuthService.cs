using Quill.Domain.Entities;

namespace Quill.Application.Interfaces.Services
{
    /// <summary>
    /// Defines the contract for security and cryptographic operations related to authentication.
    /// This service is responsible for handling password hashing, verification, and JWT generation,
    /// abstracting these security concerns away from other application services.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Converts a plain-text password into a secure, one-way hash using the BCrypt algorithm.
        /// </summary>
        /// <param name="password">The plain-text password to hash. Must not be null or whitespace.</param>
        /// <returns>A secure password hash string, including the salt, ready to be stored in the database.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the provided password is null or whitespace.</exception>
        string HashPassword(string password);

        /// <summary>
        /// Verifies if a given plain-text password matches a stored password hash.
        /// </summary>
        /// <param name="password">The plain-text password provided by the user during login.</param>
        /// <param name="hashedPassword">The stored hash from the database associated with the user account.</param>
        /// <returns>True if the password matches the hash; otherwise, false. Returns false if the hashedPassword is null or invalid.</returns>
        bool VerifyPassword(string password, string hashedPassword);

        /// <summary>
        /// Generates a JSON Web Token (JWT) for a successfully authenticated user.
        /// The token will contain essential claims like user ID, username, and role for authorization purposes.
        /// </summary>
        /// <param name="user">The user entity for whom the token will be generated. The `Role` navigation property must be loaded.</param>
        /// <returns>A string representation of the signed JWT.</returns>
        string GenerateJwtToken(User user);
    }
}