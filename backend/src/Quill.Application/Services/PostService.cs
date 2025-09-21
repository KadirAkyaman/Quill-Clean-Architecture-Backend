using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Quill.Application.DTOs.Post;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Services;
using Quill.Domain.Entities;

namespace Quill.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PostDto> CreateAsync(int authorId, PostCreateDto postCreateDto, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Post>(postCreateDto);
            post.UserId = authorId;

            await _unitOfWork.PostRepository.AddAsync(post, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var createdPostWithIncludes = await _unitOfWork.PostRepository.GetByIdAsync(post.Id, cancellationToken);
            return _mapper.Map<PostDto>(createdPostWithIncludes);
        }

        public async Task DeleteAsync(int postId, int authorId, CancellationToken cancellationToken)
        {
            var post = await GetPostAndEnsureOwnership(postId, authorId, cancellationToken);

            _unitOfWork.PostRepository.Remove(post);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<PostPreviewDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.PostRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<IReadOnlyList<PostPreviewDto>>(posts);
        }

        public async Task<IReadOnlyList<PostPreviewDto>> GetByAuthorIdAsync(int authorId, CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.PostRepository.GetByAuthorIdAsync(authorId, cancellationToken);

            return _mapper.Map<IReadOnlyList<PostPreviewDto>>(posts);
        }

        public async Task<IReadOnlyList<PostPreviewDto>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.PostRepository.GetByCategoryIdAsync(categoryId, cancellationToken);

            return _mapper.Map<IReadOnlyList<PostPreviewDto>>(posts);
        }

        public async Task<IReadOnlyList<PostPreviewDto>> GetByCategoryNameAsync(string categoryName, CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.PostRepository.GetByCategoryNameAsync(categoryName, cancellationToken);

            return _mapper.Map<IReadOnlyList<PostPreviewDto>>(posts);
        }

        public async Task<PostDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<PostDto>(post);
        }

        public async Task<IReadOnlyList<PostPreviewDto>> GetByTagNameAsync(string tagName, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetByTagNameAsync(tagName, cancellationToken);

            return _mapper.Map<IReadOnlyList<PostPreviewDto>>(post);
        }

        public async Task<IReadOnlyList<PostPreviewDto>> GetRecentAsync(int count, CancellationToken cancellationToken)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be a positive number.");
            }

            var posts = await _unitOfWork.PostRepository.GetRecentAsync(count, cancellationToken);
            return _mapper.Map<IReadOnlyList<PostPreviewDto>>(posts);
        }

        public async Task UpdateAsync(int postId, int authorId, PostUpdateDto postUpdateDto, CancellationToken cancellationToken)
        {
            var post = await GetPostAndEnsureOwnership(postId, authorId, cancellationToken);

            _mapper.Map(postUpdateDto, post);
            
            _unitOfWork.PostRepository.Update(post); 
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // HELPER
        private async Task<Post> GetPostAndEnsureOwnership(int postId, int authorId, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(postId, cancellationToken);

            if (post is null)
            {
                throw new NotFoundException($"Post with ID {postId} not found.");
            }

            if (post.UserId != authorId)
            {
                throw new UnauthorizedActionException("User is not authorized to perform this action on the post.");
            }

            return post;
        }
    }
}