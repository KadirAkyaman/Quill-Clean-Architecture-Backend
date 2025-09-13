using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Application.Interfaces.Repositories;

namespace Quill.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository Users { get; }
        public ITagRepository Tags { get; }
        public ISubscriptionRepository Subscriptions { get; }
        public IRoleRepository Roles { get; }
        public IPostRepository Posts { get; }
        public ICategoryRepository Categories { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}