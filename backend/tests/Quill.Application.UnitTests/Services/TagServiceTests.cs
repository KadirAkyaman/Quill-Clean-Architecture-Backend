using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Quill.Application.DTOs.Tag;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Repositories;
using Quill.Application.Services;
using Quill.Domain.Entities;
using Xunit;

namespace Quill.Application.UnitTests.Services
{
    public class TagServiceTests
    {
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TagService _tagService;

        public TagServiceTests()
        {
            _tagRepositoryMock = new Mock<ITagRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.TagRepository).Returns(_tagRepositoryMock.Object);

            _tagService = new TagService
            (
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );
        }

        // ---CREATE------------------------------------------------------------------------------------------

        [Fact]
        public async Task CreateAsync_WithUniqueName_ShouldCreateTag()
        {
            // Given
            var tagCreateDto = new TagCreateDto { Name = "New Tag" };
            var newTag = new Tag { Name = tagCreateDto.Name };

            _tagRepositoryMock.Setup(repo => repo.GetByNameAsync(tagCreateDto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Tag?)null);
            
            _mapperMock.Setup(m => m.Map<Tag>(tagCreateDto)).Returns(newTag);
            _mapperMock.Setup(m => m.Map<TagDto>(newTag)).Returns(new TagDto());

            // When
            await _tagService.CreateAsync(tagCreateDto, CancellationToken.None);

            // Then
            _tagRepositoryMock.Verify(repo => repo.AddAsync(newTag, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenTagNameIsTaken_ShouldThrowConflictException()
        {
            // Given
            var tagCreateDto = new TagCreateDto { Name = "Existing Tag" };

            _tagRepositoryMock.Setup(repo => repo.GetByNameAsync(tagCreateDto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Tag());

            // When
            Func<Task> act = async () => await _tagService.CreateAsync(tagCreateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ConflictException>();

            _tagRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);            
        }

        // ---UPDATE------------------------------------------------------------------------------------------
        
        [Fact]
        public async Task UpdateAsync_WhenTagExists_ShouldUpdateTag()
        {
            // Given
            var tagId = 1;
            var tagUpdateDto = new TagUpdateDto { Name = "Updated Name" };
            var tagFromDb = new Tag { Id = tagId, Name = "Original Name" };

            _tagRepositoryMock.Setup(repo => repo.GetByIdAsync(tagId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tagFromDb);
            
            _tagRepositoryMock.Setup(repo => repo.GetByNameAsync(tagUpdateDto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Tag?)null);

            _mapperMock.Setup(m => m.Map(tagUpdateDto, tagFromDb))
                .Callback<TagUpdateDto, Tag>((dto, tag) => tag.Name = dto.Name!);

            // When
            await _tagService.UpdateAsync(tagId, tagUpdateDto, CancellationToken.None);

            // Then
            _tagRepositoryMock.Verify(repo => repo.Update(It.Is<Tag>(t => t.Name == tagUpdateDto.Name)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenTagNotFound_ShouldThrowNotFoundException()
        {
            // Given
            var tagId = 999;
            var tagUpdateDto = new TagUpdateDto { Name = "Updated Name" };

            _tagRepositoryMock.Setup(repo => repo.GetByIdAsync(tagId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Tag?)null);

            // When
            Func<Task> act = async () => await _tagService.UpdateAsync(tagId, tagUpdateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<NotFoundException>();
            
            _tagRepositoryMock.Verify(repo => repo.Update(It.IsAny<Tag>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenTagNameExists_ShouldThrowConflictException()
        {
            // Given
            var tagIdToUpdate = 1;
            var newName = "Taken Name";
            var tagUpdateDto = new TagUpdateDto { Name = newName };

            var tagToUpdate = new Tag { Id = tagIdToUpdate, Name = "Original Name" };
            var otherTagWithSameName = new Tag { Id = 2, Name = newName };

            _tagRepositoryMock.Setup(repo => repo.GetByIdAsync(tagIdToUpdate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tagToUpdate);

            _tagRepositoryMock.Setup(repo => repo.GetByNameAsync(newName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(otherTagWithSameName);

            // When
            Func<Task> act = async () => await _tagService.UpdateAsync(tagIdToUpdate, tagUpdateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ConflictException>();
            
            _tagRepositoryMock.Verify(repo => repo.Update(It.Is<Tag>(t => t.Name == tagUpdateDto.Name)), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);            
        }

        // ---DELETE------------------------------------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_WhenTagExists_ShouldDeleteTag()
        {
            // Given
            var tagId = 1;
            var tagFromDb = new Tag { Id = tagId };

            _tagRepositoryMock.Setup(repo => repo.GetByIdAsync(tagId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tagFromDb);

            // When
            await _tagService.DeleteAsync(tagId, CancellationToken.None);

            // Then
            _tagRepositoryMock.Verify(repo => repo.Remove(tagFromDb), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenTagNotFound_ShouldThrowNotFoundException()
        {
            // Given
            var tagId = 999;

            _tagRepositoryMock.Setup(repo => repo.GetByIdAsync(tagId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Tag?)null);

            // When
            Func<Task> act = async () => await _tagService.DeleteAsync(tagId, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<NotFoundException>();
            
            _tagRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Tag>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);            
        }
    }
}