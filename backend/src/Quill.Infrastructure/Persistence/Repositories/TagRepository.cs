using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quill.Application.Interfaces.Repositories;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;
        public TagRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Tag tag, CancellationToken cancellationToken)
        {
            await _context.Tags.AddAsync(tag, cancellationToken);
        }

        public async Task<IReadOnlyList<Tag>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Tags.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Tag?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Tags.AsNoTracking().SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<Tag?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Tags.AsNoTracking().SingleOrDefaultAsync(t => t.Name == name, cancellationToken);
        }

        public void Remove(Tag tag)
        {
            _context.Tags.Remove(tag);
        }

        public void Update(Tag tag)
        {
            _context.Tags.Update(tag);
        }

        public async Task<List<Tag>> GetByIdsAsync(ICollection<int> ids, CancellationToken cancellationToken)
        {
            return await _context.Tags
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetPostCountAsync(int tagId, CancellationToken cancellationToken)
        {
            return await _context.PostTags
                .CountAsync(pt => pt.TagId == tagId, cancellationToken);
        }
    }
}