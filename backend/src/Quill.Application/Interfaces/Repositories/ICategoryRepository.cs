using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Domain.Entities;

namespace Quill.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken);
        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Category category, CancellationToken cancellationToken);
        void Update(Category category);
        void Remove(Category category);
    }
}