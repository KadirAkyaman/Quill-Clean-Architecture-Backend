using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Quill.Application.DTOs.Post;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Repositories;
using Quill.Application.Services;
using Quill.Domain.Entities;
using Xunit;

namespace Quill.Application.UnitTests.Services
{
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly PostService _postService;

        public PostServiceTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _tagRepositoryMock = new Mock<ITagRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.PostRepository).Returns(_postRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.TagRepository).Returns(_tagRepositoryMock.Object);

            _postService = new PostService
            (
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );

        }

        // ---CREATE------------------------------------------------------------------------------------------------------
        [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreatePostAndReturnDto()
        {
            // Given
            var authorId = 1;
            var newPostId = 123;
        
            var postCreateDto = new PostCreateDto
            {
                Title = "New Test Post",
                Text = "This is the content of the new test post.",
                Summary = "A short summary.",
                CategoryId = 10,
                TagIds = new List<int> { 1, 2 }
            };
        
            var newPost = new Post
            {
                Title = postCreateDto.Title,
                Text = postCreateDto.Text,
                Summary = postCreateDto.Summary,
                CategoryId = postCreateDto.CategoryId,
                UserId = authorId
            };
        
            var createdPostWithIncludes = new Post
            {
                Id = newPostId,
                Title = newPost.Title,
                User = new User { Id = authorId, Username = "testauthor" },
                Category = new Category { Id = newPost.CategoryId, Name = "Test Category" },
                Tags = new List<PostTag>
                {
                    new PostTag { TagId = 1, Tag = new Tag { Id = 1, Name = "Tag1" } },
                    new PostTag { TagId = 2, Tag = new Tag { Id = 2, Name = "Tag2" } }
                }
            };
        
            var existingTags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Tag1" },
                new Tag { Id = 2, Name = "Tag2" }
            };
        
            var expectedPostDto = new PostDto 
            { 
                Id = newPostId, 
                Title = newPost.Title 
            };
        
            _mapperMock.Setup(m => m.Map<Post>(postCreateDto)).Returns(newPost);
            
            _tagRepositoryMock.Setup(repo => repo.GetByIdsAsync(postCreateDto.TagIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingTags);
        
            _postRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
                .Callback<Post, CancellationToken>((post, token) => post.Id = newPostId);
        
            _postRepositoryMock.Setup(repo => repo.GetByIdAsync(newPostId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdPostWithIncludes);
            
            _mapperMock.Setup(m => m.Map<PostDto>(createdPostWithIncludes))
                .Returns(expectedPostDto);
        
            // When
            var result = await _postService.CreateAsync(authorId, postCreateDto, CancellationToken.None);
        
            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(newPostId);
            result.Title.Should().Be(postCreateDto.Title);
        
            _postRepositoryMock.Verify(repo => repo.AddAsync(
                It.Is<Post>(p => 
                    p.UserId == authorId &&
                    p.Tags.Count == postCreateDto.TagIds.Count &&
                    p.Tags.All(pt => postCreateDto.TagIds.Contains(pt.TagId))
                ), 
                It.IsAny<CancellationToken>()), 
                Times.Once);
        
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidTagIds_ShouldThrowBadRequestException()
        {
            // Given
            var authorId = 1;

            var postCreateDto = new PostCreateDto
            {
                Title = "New Test Post",
                Text = "This is the content of the new post.",
                Summary = "A short summary",
                CategoryId = 10,
                TagIds = new List<int> { 1, 999 }
            };

            var newPost = new Post
            {
                Title = postCreateDto.Title,
                Text = postCreateDto.Text,
                Summary = postCreateDto.Summary,
                CategoryId = postCreateDto.CategoryId,
                UserId = authorId
            };

            var existingTags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Tag1" }
            };

            _mapperMock.Setup(m => m.Map<Post>(postCreateDto)).Returns(newPost);

            _tagRepositoryMock.Setup(repo => repo.GetByIdsAsync(postCreateDto.TagIds, It.IsAny<CancellationToken>())).ReturnsAsync(existingTags);

            // When
            Func<Task> act = async () => await _postService.CreateAsync(authorId, postCreateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<BadRequestException>();
        
            _postRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);            
        }

        // --- UPDATE----------------------------------------------------------------------------------------------------
        [Fact]
        public async Task UpdateAsync_WhenUserIsOwner_ShouldUpdatePost()
        {
            // Given
            var authorId = 1;
            var postId = 123;

            var postUpdateDto = new PostUpdateDto
            {
                Title = "Updated Title",
                TagIds = new List<int> { 3, 4 } 
            };

            var postFromDb = new Post
            {
                Id = postId,
                UserId = authorId,
                Title = "Original Title",
                Tags = new List<PostTag>
                {
                    new PostTag { PostId = postId, TagId = 1 },
                    new PostTag { PostId = postId, TagId = 2 }
                }
            };

            var newTagsFromDb = new List<Tag>
            {
                new Tag { Id = 3, Name = "Tag3" },
                new Tag { Id = 4, Name = "Tag4" }
            };

            _postRepositoryMock.Setup(repo => repo.GetByIdForUpdateAsync(postId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(postFromDb);

            _mapperMock.Setup(m => m.Map(postUpdateDto, postFromDb))
                .Callback<PostUpdateDto, Post>((dto, post) => 
                {
                    if(dto.Title != null) post.Title = dto.Title;
                });

            _tagRepositoryMock.Setup(repo => repo.GetByIdsAsync(postUpdateDto.TagIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newTagsFromDb);

            // When
            await _postService.UpdateAsync(postId, authorId, postUpdateDto, CancellationToken.None);

            // Then
            _postRepositoryMock.Verify(repo => repo.Update(
                It.Is<Post>(p => 
                    p.Id == postId &&
                    p.Title == postUpdateDto.Title && 
                    p.Tags.Count == 2 && 
                    p.Tags.Any(pt => pt.TagId == 3) && 
                    p.Tags.Any(pt => pt.TagId == 4)
                )), 
                Times.Once);

            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenUserIsNotOwner_ShouldThrowUnauthorizedActionException()
        {
            // Given
            var ownerId = 1;
            var attackerId = 2; 
            var postId = 123;

            var postUpdateDto = new PostUpdateDto
            {
                Title = "Updated by Attacker"
            };

            var postFromDb = new Post
            {
                Id = postId,
                UserId = ownerId 
            };

            _postRepositoryMock.Setup(repo => repo.GetByIdForUpdateAsync(postId, It.IsAny<CancellationToken>())).ReturnsAsync(postFromDb);

            // When
            Func<Task> act = async () => await _postService.UpdateAsync(postId, attackerId, postUpdateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<UnauthorizedActionException>();

            _postRepositoryMock.Verify(repo => repo.Update(It.IsAny<Post>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenPostNotFound_ShouldThrowNotFoundException()
        {
            // Given
            var nonExistentPostId = 5;
            var authorId = 5;

            var postUpdateDto = new PostUpdateDto
            {
                Title = "New Test Title"
            };

            _postRepositoryMock.Setup(repo => repo.GetByIdForUpdateAsync(nonExistentPostId, It.IsAny<CancellationToken>())).ReturnsAsync((Post?)null);

            // When
            Func<Task> act = async () => await _postService.UpdateAsync(nonExistentPostId, authorId, postUpdateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<NotFoundException>();

            _postRepositoryMock.Verify(repo => repo.Update(It.IsAny<Post>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidTagIds_ShouldThrowBadRequestException()
        {
            // Given
            var authorId = 1;
            var postId = 123;

            var postUpdateDto = new PostUpdateDto
            {
                TagIds = new List<int> { 1, 999 }
            };

            var postFromDb = new Post
            {
                Id = postId,
                UserId = authorId
            };

            var validTagsFromDb = new List<Tag>
            {
                new Tag { Id = 1, Name = "Tag1" }
            };

            _postRepositoryMock.Setup(repo => repo.GetByIdForUpdateAsync(postId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(postFromDb);

            _tagRepositoryMock.Setup(repo => repo.GetByIdsAsync(postUpdateDto.TagIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validTagsFromDb);

            // When
            Func<Task> act = async () => await _postService.UpdateAsync(postId, authorId, postUpdateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<BadRequestException>();

            _postRepositoryMock.Verify(repo => repo.Update(It.IsAny<Post>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // --- DELETE-------------------------------------------------------------------------------------------------
        [Fact]
        public async Task DeleteAsync_WhenUserIsOwner_ShouldRemovePost()
        {
            // Given
            var postId = 5;
            var authorId = 10;

            var postFromDb = new Post
            {
                Id = postId,
                UserId = authorId
            };

            _postRepositoryMock.Setup(repo => repo.GetByIdForUpdateAsync(postId, It.IsAny<CancellationToken>())).ReturnsAsync(postFromDb);

            // When
            await _postService.DeleteAsync(postId, authorId, CancellationToken.None);

            // Then
            _postRepositoryMock.Verify(repo => repo.Remove(postFromDb), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenUserIsNotOwner_ShouldThrowUnauthorizedActionException()
        {
            // Given
            var ownerId = 1;
            var attackerId = 2;
            var postId = 3;

            var postFromDb = new Post
            {
                Id = postId,
                UserId = ownerId
            };

            _postRepositoryMock.Setup(repo => repo.GetByIdForUpdateAsync(postId, It.IsAny<CancellationToken>())).ReturnsAsync(postFromDb);

            // When
            Func<Task> act = async () => await _postService.DeleteAsync(postId, attackerId, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<UnauthorizedActionException>();

            _postRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Post>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WhenPostNotFound_ShouldThrowNotFoundException()
        {
            // Given
            var nonExistentPostId = 5;
            var authorId = 5;

            _postRepositoryMock.Setup(repo => repo.GetByIdForUpdateAsync(nonExistentPostId, It.IsAny<CancellationToken>())).ReturnsAsync((Post?)null);

            // When
            Func<Task> act = async () => await _postService.DeleteAsync(nonExistentPostId, authorId, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<NotFoundException>();

            _postRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Post>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // --- GET RECENT---------------------------------------------------------------------------------------------
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task GetRecentAsync_WithNonPositiveCount_ShouldThrowArgumentOutOfRangeException(int invalidCount)
        {
            // Given

            // When
            Func<Task> act = async () => await _postService.GetRecentAsync(invalidCount, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
            _postRepositoryMock.Verify(repo => repo.GetRecentAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}