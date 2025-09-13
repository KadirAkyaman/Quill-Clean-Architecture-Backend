using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Domain.Entities;

namespace Quill.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken);
        Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Role role, CancellationToken cancellationToken);
        void Update(Role role);
        void Remove(Role role);
    }
}