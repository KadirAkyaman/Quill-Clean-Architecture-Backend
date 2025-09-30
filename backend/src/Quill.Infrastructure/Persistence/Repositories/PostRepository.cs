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
            var sql = @"SELECT p.*, u.*, c.*, t.*
                        FROM ""Posts"" AS p
                        LEFT JOIN ""Users"" AS u ON p.""UserId"" = u.""Id""
                        LEFT JOIN ""Categories"" AS c ON p.""CategoryId"" = c.""Id""
                        LEFT JOIN ""PostTags"" AS pt ON p.""Id"" = pt.""PostId""
                        LEFT JOIN ""Tags"" AS t ON pt.""TagId"" = t.""Id""";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
            var postDictionary = new Dictionary<int, Post>();

            await connection.QueryAsync<Post, User, Category, Tag, Post>(
                new CommandDefinition(sql, transaction: transaction, cancellationToken: cancellationToken),
                (post, user, category, tag) =>
                {
                    if (!postDictionary.TryGetValue(post.Id, out var currentPost))
                    {
                        currentPost = post;
                        currentPost.User = user;
                        currentPost.Category = category;
                        currentPost.Tags = new List<PostTag>();
                        postDictionary.Add(currentPost.Id, currentPost);
                    }
                    if (tag != null && !currentPost.Tags.Any(pt => pt.TagId == tag.Id))
                    {
                        currentPost.Tags.Add(new PostTag { Post = currentPost, Tag = tag, TagId = tag.Id });
                    }
                    return currentPost;
                },
                splitOn: "Id,Id,Id,Id"
            );
            return postDictionary.Values.ToList();
        }

        public async Task<IReadOnlyList<Post>> GetByAuthorIdAsync(int authorId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT p.*, u.*, c.*, t.*
                        FROM ""Posts"" AS p
                        LEFT JOIN ""Users"" AS u ON p.""UserId"" = u.""Id""
                        LEFT JOIN ""Categories"" AS c ON p.""CategoryId"" = c.""Id""
                        LEFT JOIN ""PostTags"" AS pt ON p.""Id"" = pt.""PostId""
                        LEFT JOIN ""Tags"" AS t ON pt.""TagId"" = t.""Id""
                        WHERE p.""UserId"" = @AuthorId";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
            var postDictionary = new Dictionary<int, Post>();

            await connection.QueryAsync<Post, User, Category, Tag, Post>(
                new CommandDefinition(sql, new { AuthorId = authorId }, transaction, cancellationToken: cancellationToken),
                (post, user, category, tag) =>
                {
                    if (!postDictionary.TryGetValue(post.Id, out var currentPost))
                    {
                        currentPost = post;
                        currentPost.User = user;
                        currentPost.Category = category;
                        currentPost.Tags = new List<PostTag>();
                        postDictionary.Add(currentPost.Id, currentPost);
                    }
                    if (tag != null && !currentPost.Tags.Any(pt => pt.TagId == tag.Id))
                    {
                        currentPost.Tags.Add(new PostTag { Post = currentPost, Tag = tag, TagId = tag.Id });
                    }
                    return currentPost;
                },
                splitOn: "Id,Id,Id,Id"
            );
            return postDictionary.Values.ToList();
        }

        public async Task<IReadOnlyList<Post>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT p.*, u.*, c.*, t.*
                        FROM ""Posts"" AS p
                        LEFT JOIN ""Users"" AS u ON p.""UserId"" = u.""Id""
                        LEFT JOIN ""Categories"" AS c ON p.""CategoryId"" = c.""Id""
                        LEFT JOIN ""PostTags"" AS pt ON p.""Id"" = pt.""PostId""
                        LEFT JOIN ""Tags"" AS t ON pt.""TagId"" = t.""Id""
                        WHERE p.""CategoryId"" = @CategoryId";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
            var postDictionary = new Dictionary<int, Post>();

            await connection.QueryAsync<Post, User, Category, Tag, Post>(
                new CommandDefinition(sql, new { CategoryId = categoryId }, transaction, cancellationToken: cancellationToken),
                (post, user, category, tag) =>
                {
                    if (!postDictionary.TryGetValue(post.Id, out var currentPost))
                    {
                        currentPost = post;
                        currentPost.User = user;
                        currentPost.Category = category;
                        currentPost.Tags = new List<PostTag>();
                        postDictionary.Add(currentPost.Id, currentPost);
                    }
                    if (tag != null && !currentPost.Tags.Any(pt => pt.TagId == tag.Id))
                    {
                        currentPost.Tags.Add(new PostTag { Post = currentPost, Tag = tag, TagId = tag.Id });
                    }
                    return currentPost;
                },
                splitOn: "Id,Id,Id,Id"
            );
            return postDictionary.Values.ToList();
        }

        public async Task<IReadOnlyList<Post>> GetByCategoryNameAsync(string categoryName, CancellationToken cancellationToken)
        {
            var sql = @"SELECT p.*, u.*, c.*, t.*
                        FROM ""Posts"" AS p
                        LEFT JOIN ""Users"" AS u ON p.""UserId"" = u.""Id""
                        LEFT JOIN ""Categories"" AS c ON p.""CategoryId"" = c.""Id""
                        LEFT JOIN ""PostTags"" AS pt ON p.""Id"" = pt.""PostId""
                        LEFT JOIN ""Tags"" AS t ON pt.""TagId"" = t.""Id""
                        WHERE c.""Name"" = @CategoryName";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
            var postDictionary = new Dictionary<int, Post>();

            await connection.QueryAsync<Post, User, Category, Tag, Post>(
                new CommandDefinition(sql, new { CategoryName = categoryName }, transaction, cancellationToken: cancellationToken),
                (post, user, category, tag) =>
                {
                    if (!postDictionary.TryGetValue(post.Id, out var currentPost))
                    {
                        currentPost = post;
                        currentPost.User = user;
                        currentPost.Category = category;
                        currentPost.Tags = new List<PostTag>();
                        postDictionary.Add(currentPost.Id, currentPost);
                    }
                    if (tag != null && !currentPost.Tags.Any(pt => pt.TagId == tag.Id))
                    {
                        currentPost.Tags.Add(new PostTag { Post = currentPost, Tag = tag, TagId = tag.Id });
                    }
                    return currentPost;
                },
                splitOn: "Id,Id,Id,Id"
            );
            return postDictionary.Values.ToList();
        }

        public async Task<Post?> GetByIdAsync(int postId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT p.*, u.*, c.*, t.*
                        FROM ""Posts"" AS p
                        LEFT JOIN ""Users"" AS u ON p.""UserId"" = u.""Id""
                        LEFT JOIN ""Categories"" AS c ON p.""CategoryId"" = c.""Id""
                        LEFT JOIN ""PostTags"" AS pt ON p.""Id"" = pt.""PostId""
                        LEFT JOIN ""Tags"" AS t ON pt.""TagId"" = t.""Id""
                        WHERE p.""Id"" = @PostId";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
            var postDictionary = new Dictionary<int, Post>();

            await connection.QueryAsync<Post, User, Category, Tag, Post>(
                new CommandDefinition(sql, new { PostId = postId }, transaction, cancellationToken: cancellationToken),
                (post, user, category, tag) =>
                {
                    if (!postDictionary.TryGetValue(post.Id, out var currentPost))
                    {
                        currentPost = post;
                        currentPost.User = user;
                        currentPost.Category = category;
                        currentPost.Tags = new List<PostTag>();
                        postDictionary.Add(currentPost.Id, currentPost);
                    }
                    if (tag != null && !currentPost.Tags.Any(pt => pt.TagId == tag.Id))
                    {
                        currentPost.Tags.Add(new PostTag { Post = currentPost, Tag = tag, TagId = tag.Id });
                    }
                    return currentPost;
                },
                splitOn: "Id,Id,Id,Id"
            );
            return postDictionary.Values.FirstOrDefault();
        }

        public async Task<IReadOnlyList<Post>> GetByTagNameAsync(string tagName, CancellationToken cancellationToken)
        {
            var sql = @"SELECT p.*, u.*, c.*, t.*
                        FROM ""Posts"" p
                        LEFT JOIN ""Users"" u ON p.""UserId"" = u.""Id""
                        LEFT JOIN ""Categories"" c ON p.""CategoryId"" = c.""Id""
                        INNER JOIN ""PostTags"" pt ON p.""Id"" = pt.""PostId""
                        INNER JOIN ""Tags"" t_filter ON pt.""TagId"" = t_filter.""Id"" AND t_filter.""Name"" = @TagName
                        LEFT JOIN ""PostTags"" pt_all ON p.""Id"" = pt_all.""PostId""
                        LEFT JOIN ""Tags"" t ON pt_all.""TagId"" = t.""Id""";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
            var postDictionary = new Dictionary<int, Post>();

            await connection.QueryAsync<Post, User, Category, Tag, Post>(
                new CommandDefinition(sql, new { TagName = tagName }, transaction, cancellationToken: cancellationToken),
                (post, user, category, tag) =>
                {
                    if (!postDictionary.TryGetValue(post.Id, out var currentPost))
                    {
                        currentPost = post;
                        currentPost.User = user;
                        currentPost.Category = category;
                        currentPost.Tags = new List<PostTag>();
                        postDictionary.Add(currentPost.Id, currentPost);
                    }
                    if (tag != null && !currentPost.Tags.Any(pt => pt.TagId == tag.Id))
                    {
                        currentPost.Tags.Add(new PostTag { Post = currentPost, Tag = tag, TagId = tag.Id });
                    }
                    return currentPost;
                },
                splitOn: "Id,Id,Id,Id"
            );
            return postDictionary.Values.ToList();
        }

        public async Task<int> GetCountByAuthorIdAsync(int authorId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT COUNT(*) FROM ""Posts"" WHERE ""UserId"" = @AuthorId";

            var connection = _context.Database.GetDbConnection();

            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

            return await connection.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { AuthorId = authorId }, transaction: transaction, cancellationToken: cancellationToken));
        }

        public async Task<IReadOnlyList<Post>> GetRecentByAuthorIdAsync(int authorId, int count, CancellationToken cancellationToken)
        {
            var sql = @"
                WITH RecentPosts AS (
                    SELECT * FROM ""Posts""
                    WHERE ""UserId"" = @AuthorId
                    ORDER BY ""CreatedAt"" DESC
                    LIMIT @Count
                )
                SELECT p.*, u.*, c.*, t.*
                FROM RecentPosts p
                LEFT JOIN ""Users"" u ON p.""UserId"" = u.""Id""
                LEFT JOIN ""Categories"" c ON p.""CategoryId"" = c.""Id""
                LEFT JOIN ""PostTags"" pt ON p.""Id"" = pt.""PostId""
                LEFT JOIN ""Tags"" t ON pt.""TagId"" = t.""Id""
                ORDER BY p.""CreatedAt"" DESC";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
            var postDictionary = new Dictionary<int, Post>();

            await connection.QueryAsync<Post, User, Category, Tag, Post>(
                new CommandDefinition(sql, new { AuthorId = authorId, Count = count }, transaction, cancellationToken: cancellationToken),
                (post, user, category, tag) =>
                {
                    if (!postDictionary.TryGetValue(post.Id, out var currentPost))
                    {
                        currentPost = post;
                        currentPost.User = user;
                        currentPost.Category = category;
                        currentPost.Tags = new List<PostTag>();
                        postDictionary.Add(currentPost.Id, currentPost);
                    }
                    if (tag != null && !currentPost.Tags.Any(pt => pt.TagId == tag.Id))
                    {
                        currentPost.Tags.Add(new PostTag { Post = currentPost, Tag = tag, TagId = tag.Id });
                    }
                    return currentPost;
                },
                splitOn: "Id,Id,Id,Id"
            );
            return postDictionary.Values.ToList();
        }

        public async Task<IReadOnlyList<Post>> GetRecentAsync(int count, CancellationToken cancellationToken)
        {
            var sql = @"
                WITH RecentPosts AS (
                    SELECT * FROM ""Posts""
                    ORDER BY ""CreatedAt"" DESC
                    LIMIT @Count
                )
                SELECT p.*, u.*, c.*, t.*
                FROM RecentPosts p
                LEFT JOIN ""Users"" u ON p.""UserId"" = u.""Id""
                LEFT JOIN ""Categories"" c ON p.""CategoryId"" = c.""Id""
                LEFT JOIN ""PostTags"" pt ON p.""Id"" = pt.""PostId""
                LEFT JOIN ""Tags"" t ON pt.""TagId"" = t.""Id""
                ORDER BY p.""CreatedAt"" DESC";

            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
            var postDictionary = new Dictionary<int, Post>();

            await connection.QueryAsync<Post, User, Category, Tag, Post>(
                new CommandDefinition(sql, new { Count = count }, transaction, cancellationToken: cancellationToken),
                (post, user, category, tag) =>
                {
                    if (!postDictionary.TryGetValue(post.Id, out var currentPost))
                    {
                        currentPost = post;
                        currentPost.User = user;
                        currentPost.Category = category;
                        currentPost.Tags = new List<PostTag>();
                        postDictionary.Add(currentPost.Id, currentPost);
                    }
                    if (tag != null && !currentPost.Tags.Any(pt => pt.TagId == tag.Id))
                    {
                        currentPost.Tags.Add(new PostTag { Post = currentPost, Tag = tag, TagId = tag.Id });
                    }
                    return currentPost;
                },
                splitOn: "Id,Id,Id,Id"
            );
            return postDictionary.Values.ToList();
        }

        public void Remove(Post post)
        {
            _context.Posts.Remove(post);
        }

        public void Update(Post post)
        {
            _context.Posts.Update(post);
        }

        public async Task<int> GetCountByCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT COUNT(*) FROM ""Posts"" WHERE ""CategoryId"" = @CategoryId";
            var connection = _context.Database.GetDbConnection();
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
            return await connection.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { CategoryId = categoryId }, transaction, cancellationToken: cancellationToken));
        }
        
        public async Task<Post?> GetByIdForUpdateAsync(int postId, CancellationToken cancellationToken)
        {
            return await _context.Posts.Include(p => p.Tags).SingleOrDefaultAsync(p => p.Id == postId, cancellationToken);
        }
    }
}