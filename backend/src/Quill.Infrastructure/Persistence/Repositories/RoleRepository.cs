using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quill.Application.Interfaces.Repositories;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Role role, CancellationToken cancellationToken)
        {
            await _context.Roles.AddAsync(role, cancellationToken);
        }

        public async Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Roles.Include(r => r.Users).AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Roles.Include(r => r.Users).AsNoTracking().SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Roles.Include(r => r.Users).AsNoTracking().SingleOrDefaultAsync(r => r.Name == name, cancellationToken);
        }

        public void Remove(Role role)
        {
            _context.Roles.Remove(role);
        }

        public void Update(Role role)
        {
            _context.Roles.Update(role);
        }    
    }
}