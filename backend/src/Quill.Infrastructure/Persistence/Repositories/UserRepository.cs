using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quill.Application.Interfaces.Repositories;
using Quill.Domain.Entities;

namespace Quill.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == email ,cancellationToken);
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id ,cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Username == username ,cancellationToken);
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
    }
}