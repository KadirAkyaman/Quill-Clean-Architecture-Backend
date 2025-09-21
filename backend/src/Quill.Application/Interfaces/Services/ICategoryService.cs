using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Application.DTOs.Category;

namespace Quill.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<CategoryDto?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<CategoryDto> CreateAsync(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken);
        Task UpdateAsync(int id ,CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}