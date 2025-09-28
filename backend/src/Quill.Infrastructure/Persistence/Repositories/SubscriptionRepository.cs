using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            var sql = @"SELECT * 
                        FROM ""Subscriptions"" 
                        WHERE ""SubscribedToId"" = @SubscribedToId 
                          AND ""SubscriberId"" = @SubscriberId";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

            return await connection.QuerySingleOrDefaultAsync<Subscription>(
                new CommandDefinition(sql, new { SubscribedToId = subscribedToId, SubscriberId = subscriberId }, 
                transaction: transaction, cancellationToken: cancellationToken));
        }

        public async Task<int> GetSubscriberCountAsync(int userId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT COUNT(*) 
                        FROM ""Subscriptions"" 
                        WHERE ""SubscribedToId"" = @UserId 
                          AND ""IsActive"" = true";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

            return await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(sql, new { UserId = userId }, transaction: transaction, cancellationToken: cancellationToken));
        }

        public async Task<int> GetSubscriptionCountAsync(int userId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT COUNT(*) 
                        FROM ""Subscriptions"" 
                        WHERE ""SubscriberId"" = @UserId 
                          AND ""IsActive"" = true";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

            return await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(sql, new { UserId = userId }, transaction: transaction, cancellationToken: cancellationToken));
        }

        public async Task<IReadOnlyList<Subscription>> GetSubscribersBySubscribedToIdAsync(int subscribedToId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT s.*, subscriber.*, subscribedTo.*
                        FROM ""Subscriptions"" AS s
                        INNER JOIN ""Users"" AS subscriber ON s.""SubscriberId"" = subscriber.""Id""
                        INNER JOIN ""Users"" AS subscribedTo ON s.""SubscribedToId"" = subscribedTo.""Id""
                        WHERE s.""SubscribedToId"" = @SubscribedToId
                          AND s.""IsActive"" = true";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

            var subscribers = await connection.QueryAsync<Subscription, User, User, Subscription>(
                new CommandDefinition(sql, new { SubscribedToId = subscribedToId }, transaction, cancellationToken: cancellationToken),
                (subscription, subscriberUser, subscribedToUser) =>
                {
                    subscription.Subscriber = subscriberUser;
                    subscription.SubscribedTo = subscribedToUser;
                    return subscription;
                },
                splitOn: "Id,Id"
            );

            return subscribers.ToList();
        }

        public async Task<IReadOnlyList<Subscription>> GetSubscriptionsBySubscriberIdAsync(int subscriberId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT s.*, subscriber.*, subscribedTo.*
                        FROM ""Subscriptions"" AS s
                        INNER JOIN ""Users"" AS subscriber ON s.""SubscriberId"" = subscriber.""Id""
                        INNER JOIN ""Users"" AS subscribedTo ON s.""SubscribedToId"" = subscribedTo.""Id""
                        WHERE s.""SubscriberId"" = @SubscriberId
                          AND s.""IsActive"" = true";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

            var subscriptions = await connection.QueryAsync<Subscription, User, User, Subscription>(
                new CommandDefinition(sql, new { SubscriberId = subscriberId }, transaction, cancellationToken: cancellationToken),
                (subscription, subscriberUser, subscribedToUser) =>
                {
                    subscription.Subscriber = subscriberUser;
                    subscription.SubscribedTo = subscribedToUser;
                    return subscription;
                },
                splitOn: "Id,Id"
            );

            return subscriptions.ToList();
        }

        public void Update(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
        }

        public async Task<Subscription?> GetByIdWithDetailsAsync(int subscriptionId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT s.*, subscriber.*, subscribedTo.*
                        FROM ""Subscriptions"" AS s
                        INNER JOIN ""Users"" AS subscriber ON s.""SubscriberId"" = subscriber.""Id""
                        INNER JOIN ""Users"" AS subscribedTo ON s.""SubscribedToId"" = subscribedTo.""Id""
                        WHERE s.""Id"" = @SubscriptionId";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

            var subscriptions = await connection.QueryAsync<Subscription, User, User, Subscription>(
                new CommandDefinition(sql, new { SubscriptionId = subscriptionId }, transaction, cancellationToken: cancellationToken),
                (subscription, subscriberUser, subscribedToUser) =>
                {
                    subscription.Subscriber = subscriberUser;
                    subscription.SubscribedTo = subscribedToUser;
                    return subscription;
                },
                splitOn: "Id,Id"
            );

            return subscriptions.SingleOrDefault();
        }
    }
}
