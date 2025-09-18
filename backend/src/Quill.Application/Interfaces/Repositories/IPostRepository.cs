using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Domain.Entities;

namespace Quill.Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<IReadOnlyList<Post>> GetAllAsync(CancellationToken cancellationToken);
        Task<Post?> GetByIdAsync(int postId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Post>> GetByAuthorIdAsync(int authorId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Post>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Post>> GetByCategoryNameAsync(string categoryName, CancellationToken cancellationToken);
        Task<IReadOnlyList<Post>> GetByTagNameAsync(string tagName, CancellationToken cancellationToken);
        Task<IReadOnlyList<Post>> GetRecentAsync(int count, CancellationToken cancellationToken);//Retrieves the most recently created posts up to a specified number. (e.g., for the homepage)
        Task AddAsync(Post post, CancellationToken cancellationToken);
        void Update(Post post);
        void Remove(Post post);
    }
}