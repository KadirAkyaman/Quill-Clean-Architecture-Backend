using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Repositories;
using Quill.Infrastructure.Persistence.Repositories;

namespace Quill.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository UserRepository { get; }
        public ITagRepository TagRepository { get; }
        public ISubscriptionRepository SubscriptionRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IPostRepository PostRepository { get; }
        public ICategoryRepository CategoryRepository { get; }

        public UnitOfWork(AppDbContext context, IUserRepository userRepository, ITagRepository tagRepository, ISubscriptionRepository subscriptionRepository, IRoleRepository roleRepository, IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _context = context;
            UserRepository = userRepository;
            TagRepository = tagRepository;
            SubscriptionRepository = subscriptionRepository;
            RoleRepository = roleRepository;
            PostRepository = postRepository;
            CategoryRepository = categoryRepository;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}