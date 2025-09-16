using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Application.Interfaces.Repositories;

namespace Quill.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; }
        public ITagRepository TagRepository { get; }
        public ISubscriptionRepository SubscriptionRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IPostRepository PostRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}