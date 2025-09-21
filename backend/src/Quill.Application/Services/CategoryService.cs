using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Quill.Application.DTOs.Category;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Services;
using Quill.Domain.Entities;

namespace Quill.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto> CreateAsync(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(categoryCreateDto);

            await _unitOfWork.CategoryRepository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var category = await GetCategoryAndEnsureExists(id, cancellationToken);

            _unitOfWork.CategoryRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetByNameAsync(name, cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateAsync(int id, CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken)
        {
            var category = await GetCategoryAndEnsureExists(id, cancellationToken);

            _mapper.Map(categoryUpdateDto, category);
            _unitOfWork.CategoryRepository.Update(category); 
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }


        //HELPER
        private async Task<Category> GetCategoryAndEnsureExists(int id, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id, cancellationToken);
            if (category is null)
               throw new NotFoundException($"Category with ID {id} not found.");

            return category;
        }
    }
}