using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Domain.Entities;

namespace Quill.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        Task AddAsync(User user, CancellationToken cancellationToken);
        void Update(User user);
    }
}