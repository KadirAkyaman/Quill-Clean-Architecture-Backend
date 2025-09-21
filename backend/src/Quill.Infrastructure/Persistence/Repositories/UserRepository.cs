using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            var sql = @"SELECT * FROM ""Users""";

            using (var connection = _context.Database.GetDbConnection())
            {
                var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
                var users = await connection.QueryAsync<User>(new CommandDefinition(sql, transaction: transaction, cancellationToken: cancellationToken));

                return users.ToList();
            }
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var sql = @"SELECT u.*, r.* 
                        FROM ""Users"" AS u
                        LEFT JOIN ""Roles"" AS r ON u.""RoleId"" = r.""Id""
                        WHERE u.""Email"" = @Email";

            using (var connection = _context.Database.GetDbConnection())
            {
                var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

                var users = await connection.QueryAsync<User, Role, User>(
                    new CommandDefinition(sql, new { Email = email }, transaction, cancellationToken: cancellationToken),
                    (user, role) => 
                    {
                        user.Role = role;
                        return user;
                    },
                    splitOn: "Id"
                );

                return users.SingleOrDefault();
            }
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var sql = @"SELECT * FROM ""Users"" WHERE ""Id"" = @Id";

            using (var connection = _context.Database.GetDbConnection())
            {
                var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
                return await connection.QuerySingleOrDefaultAsync<User>(new CommandDefinition(sql, new { Id = id }, transaction: transaction, cancellationToken: cancellationToken));
            }
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
           var sql = @"SELECT u.*, r.* 
                       FROM ""Users"" AS u
                       LEFT JOIN ""Roles"" AS r ON u.""RoleId"" = r.""Id""
                       WHERE u.""Username"" = @Username";

           using (var connection = _context.Database.GetDbConnection())
           {
               var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

               var users = await connection.QueryAsync<User, Role, User>(
                   new CommandDefinition(sql, new { Username = username }, transaction, cancellationToken: cancellationToken),
                   (user, role) =>
                   {
                       user.Role = role;
                       return user;
                   },
                   splitOn: "Id"
               );

               return users.SingleOrDefault();
           }
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
    }
}