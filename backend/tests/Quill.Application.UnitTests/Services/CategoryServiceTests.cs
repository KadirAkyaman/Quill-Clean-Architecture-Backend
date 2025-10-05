using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Quill.Application.DTOs.Category;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Repositories;
using Quill.Application.Services;
using Quill.Domain.Entities;
using Xunit;

namespace Quill.Application.UnitTests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository).Returns(_categoryRepositoryMock.Object);

            _categoryService = new CategoryService(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        // ---CREATE-------------------------------------------------------------------------------------------------------  

        [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreateCategory()
        {
            // Given
            var categoryCreateDto = new CategoryCreateDto { Name = "New Category" };
            var newCategory = new Category { Name = categoryCreateDto.Name };

            _mapperMock.Setup(m => m.Map<Category>(categoryCreateDto)).Returns(newCategory);
            _mapperMock.Setup(m => m.Map<CategoryDto>(newCategory)).Returns(new CategoryDto());

            // When
            await _categoryService.CreateAsync(categoryCreateDto, CancellationToken.None);

            // Then
            _categoryRepositoryMock.Verify(repo => repo.AddAsync(newCategory, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenCategoryNameExists_ShouldThrowConflictException()
        {
            // Given
            var categoryCreateDto = new CategoryCreateDto { Name = "Existing Category" };
            var existingCategory = new Category { Id = 1, Name = categoryCreateDto.Name };

            _categoryRepositoryMock.Setup(repo => repo.GetByNameAsync(categoryCreateDto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            // When
            Func<Task> act = async () => await _categoryService.CreateAsync(categoryCreateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ConflictException>();
            _categoryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // ---UPDATE-------------------------------------------------------------------------------------------------------  

        [Fact]
        public async Task UpdateAsync_WhenCategoryExists_ShouldUpdateCategory()
        {
            // Given
            var categoryId = 1;
            var categoryUpdateDto = new CategoryUpdateDto { Name = "Updated Name" };
            var categoryFromDb = new Category { Id = categoryId, Name = "Original Name" };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(categoryFromDb);

            _mapperMock.Setup(m => m.Map(categoryUpdateDto, categoryFromDb))
            .Callback<CategoryUpdateDto, Category>((dto, category) => 
            {
                category.Name = dto.Name!;
            });

            // When
            await _categoryService.UpdateAsync(categoryId, categoryUpdateDto, CancellationToken.None);

            // Then
            _categoryRepositoryMock.Verify(repo => repo.Update(It.Is<Category>(c => c.Name == categoryUpdateDto.Name)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryNotFound_ShouldThrowNotFoundException()
        {
            // Given
            var categoryId = 999;
            var categoryUpdateDto = new CategoryUpdateDto { Name = "Updated Name" };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            // When
            Func<Task> act = async () => await _categoryService.UpdateAsync(categoryId, categoryUpdateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<NotFoundException>();
            
            _categoryRepositoryMock.Verify(repo => repo.Update(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryNameExists_ShouldThrowConflictException()
        {
            // Given
            var categoryId = 1;
            var categoryUpdateDto = new CategoryUpdateDto { Name = "Existing Category" };
            var categoryFromDb = new Category { Id = categoryId, Name = "Original Name" };
            var conflictingCategory = new Category { Id = 2, Name = categoryUpdateDto.Name };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(categoryFromDb);

            _categoryRepositoryMock.Setup(repo => repo.GetByNameAsync(categoryUpdateDto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(conflictingCategory);

            // When
            Func<Task> act = async () => await _categoryService.UpdateAsync(categoryId, categoryUpdateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ConflictException>();
            
            _categoryRepositoryMock.Verify(repo => repo.Update(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // ---DELETE-------------------------------------------------------------------------------------------------------
        [Fact]
        public async Task DeleteAsync_WhenCategoryExists_ShouldDeleteCategory()
        {
            // Given
            var categoryId = 1;
            var categoryFromDb = new Category { Id = categoryId };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(categoryFromDb);

            // When
            await _categoryService.DeleteAsync(categoryId, CancellationToken.None);

            // Then
            _categoryRepositoryMock.Verify(repo => repo.Remove(categoryFromDb), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenCategoryNotFound_ShouldThrowNotFoundException()
        {
            // Given
            var categoryId = 999;

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            // When
            Func<Task> act = async () => await _categoryService.DeleteAsync(categoryId, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<NotFoundException>();
            
            _categoryRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}