using Quill.Domain.Entities;

namespace Quill.Application.Interfaces.Repositories
{
    /// <summary>
    /// Defines the contract for data operations on the User entity.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a read-only list of all users from the database.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a read-only list of all users.</returns>
        Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single user by their unique identifier, including their role.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the user if found; otherwise, null.</returns>
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single user by their unique email address, including their role.
        /// This is primarily used for internal processes like login and registration checks.
        /// </summary>
        /// <param name="email">The unique email address of the user.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the user if found; otherwise, null.</returns>
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single user by their unique username, including their role.
        /// This is used for accessing public user profiles.
        /// </summary>
        /// <param name="username">The unique username of the user.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the user if found; otherwise, null.</returns>
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);

        /// <summary>
        /// Marks a new user to be added to the database.
        /// </summary>
        /// <param name="user">The new user entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Marks an existing user as modified in the change tracker (e.g., for profile updates or soft deletes).
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        void Update(User user);
    }
}