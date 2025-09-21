using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Quill.Application.DTOs.Post;

namespace Quill.Application.Interfaces.Services
{
    public interface IPostService
    {
        Task<IReadOnlyList<PostPreviewDto>> GetAllAsync(CancellationToken cancellationToken);
        
        Task<PostDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        
        Task<IReadOnlyList<PostPreviewDto>> GetByAuthorIdAsync(int authorId, CancellationToken cancellationToken);
        
        Task<IReadOnlyList<PostPreviewDto>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken);
        
        Task<IReadOnlyList<PostPreviewDto>> GetByCategoryNameAsync(string categoryName, CancellationToken cancellationToken);
        
        Task<IReadOnlyList<PostPreviewDto>> GetByTagNameAsync(string tagName, CancellationToken cancellationToken);
        
        Task<IReadOnlyList<PostPreviewDto>> GetRecentAsync(int count, CancellationToken cancellationToken);
        
        //This authorId parameter is the most fundamental and secure method that enables us to implement the “authorization” step (“do you have permission to do this?”) that follows the ‘authentication’ step (“who are you?”).
        Task<PostDto> CreateAsync(int authorId, PostCreateDto postCreateDto, CancellationToken cancellationToken);
        
        Task UpdateAsync(int postId, int authorId, PostUpdateDto postUpdateDto, CancellationToken cancellationToken);
        
        Task DeleteAsync(int postId, int authorId, CancellationToken cancellationToken); // A user should only be able to delete their own posts.
    }
}