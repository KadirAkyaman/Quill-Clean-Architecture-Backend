using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Application.DTOs.Tag;

namespace Quill.Application.Interfaces.Services
{
    public interface ITagService
    {
        Task<IReadOnlyList<TagDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<TagDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<TagDto?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<TagDto> CreateAsync(TagCreateDto tagCreateDto, CancellationToken cancellationToken);
        Task UpdateAsync(int id, TagUpdateDto tagUpdateDto, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}