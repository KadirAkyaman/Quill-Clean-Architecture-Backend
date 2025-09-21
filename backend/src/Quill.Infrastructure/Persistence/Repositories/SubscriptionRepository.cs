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
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _context;

        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken)
        {
            await _context.Subscriptions.AddAsync(subscription, cancellationToken);
        }

        public async Task<Subscription?> FindSubscriptionAsync(int subscriberId, int subscribedToId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT * FROM ""Subscriptions"" WHERE ""SubscribedToId"" = @SubscribedToId AND ""SubscriberId"" = @SubscriberId";

            using (var connection = _context.Database.GetDbConnection())
            {
                var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
                return await connection.QuerySingleOrDefaultAsync<Subscription>(new CommandDefinition(sql, new { SubscribedToId = subscribedToId, SubscriberId = subscriberId }, transaction: transaction, cancellationToken: cancellationToken));
            }

            //return await _context.Subscriptions.AsNoTracking().SingleOrDefaultAsync(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId, cancellationToken);
        }

        public async Task<int> GetSubscriberCountAsync(int userId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT COUNT(*) FROM ""Subscriptions"" WHERE ""SubscribedToId"" = @UserId AND ""IsActive"" = true";

            using (var connection = _context.Database.GetDbConnection())
            {
                var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

                return await connection.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { UserId = userId }, transaction: transaction, cancellationToken: cancellationToken));
            }
        }

        public async Task<int> GetSubscriptionCountAsync(int userId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT COUNT(*) FROM ""Subscriptions"" WHERE ""SubscriberId"" = @UserId AND ""IsActive"" = true";

            using (var connection = _context.Database.GetDbConnection())
            {
                var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

                return await connection.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { UserId = userId }, transaction: transaction, cancellationToken: cancellationToken));
            }
        }

        public async Task<IReadOnlyList<Subscription>> GetSubscribersBySubscribedToIdAsync(int subscribedToId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT * FROM ""Subscriptions"" WHERE ""SubscribedToId"" = @SubscribedToId";

            using (var connection = _context.Database.GetDbConnection())
            {
                var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

                var subscribers = await connection.QueryAsync<Subscription>(new CommandDefinition(sql, new { SubscribedToId = subscribedToId }, transaction: transaction, cancellationToken: cancellationToken));

                return subscribers.ToList();
            }
            //return await _context.Subscriptions.AsNoTracking().Where(s => s.SubscribedToId == subscribedToId).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Subscription>> GetSubscriptionsBySubscriberIdAsync(int subscriberId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT * FROM ""Subscriptions"" WHERE ""SubscriberId"" = @SubscriberId";

            using (var connection = _context.Database.GetDbConnection())
            {
                var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

                var subscriptions = await connection.QueryAsync<Subscription>(new CommandDefinition(sql, new { SubscriberId = subscriberId }, transaction: transaction, cancellationToken: cancellationToken));

                return subscriptions.ToList();
            }

            //return await _context.Subscriptions.AsNoTracking().Where(s => s.SubscriberId == subscriberId).ToListAsync(cancellationToken);
        }

        public void Update(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
        }
    }
}