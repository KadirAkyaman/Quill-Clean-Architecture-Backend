using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Domain.Entities;

namespace Quill.Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<IReadOnlyList<Tag>> GetAllAsync(CancellationToken cancellationToken);
        Task<Tag?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Tag?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Tag tag, CancellationToken cancellationToken);
        void Update(Tag tag);
        void Remove(Tag tag);
    }
}