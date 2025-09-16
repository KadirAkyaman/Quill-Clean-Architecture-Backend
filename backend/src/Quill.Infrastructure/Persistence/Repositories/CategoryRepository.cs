using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quill.Application.Interfaces.Repositories;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Category category, CancellationToken cancellationToken)
        {
            await _context.Categories.AddAsync(category, cancellationToken);
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking().SingleOrDefaultAsync(c => c.Name == name, cancellationToken);
        }

        public void Remove(Category category)
        {
            _context.Categories.Remove(category);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}