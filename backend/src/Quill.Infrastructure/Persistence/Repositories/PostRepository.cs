using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quill.Application.Interfaces.Repositories;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;
        public PostRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Post post, CancellationToken cancellationToken)
        {
            await _context.Posts.AddAsync(post, cancellationToken);
        }

        public async Task<IReadOnlyList<Post>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Posts.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Post>> GetByAuthorIdAsync(int authorId, CancellationToken cancellationToken)
        {
            return await _context.Posts.AsNoTracking().Include(p => p.Category).Where(p => p.UserId == authorId).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Post>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
        {
            return await _context.Posts.AsNoTracking().Where(p => p.CategoryId == categoryId).ToListAsync(cancellationToken);
        }

        public async Task<Post?> GetByIdAsync(int postId, CancellationToken cancellationToken)
        {
            return await _context.Posts.AsNoTracking().SingleOrDefaultAsync(p => p.Id == postId, cancellationToken);
        }

        public async Task<IReadOnlyList<Post>> GetByTagNameAsync(string tagName, CancellationToken cancellationToken)
        {
            return await _context.Posts.AsNoTracking().Where(p => p.Tags.Any(pt => pt.Tag.Name == tagName)).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Post>> GetRecentAsync(int count, CancellationToken cancellationToken)
        {
            return await _context.Posts.AsNoTracking().Include(p => p.User).Include(p => p.Category).OrderByDescending(p => p.CreatedAt).Take(count).ToListAsync(cancellationToken);
        }

        public void Remove(Post post)
        {
            _context.Posts.Remove(post);
        }

        public void Update(Post post)
        {
            _context.Posts.Update(post);
        }
    }
}