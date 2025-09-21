using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Application.DTOs.Role;

namespace Quill.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<IReadOnlyList<RoleDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<RoleDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<RoleDto?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<RoleDto> CreateAsync(RoleCreateDto roleCreateDto, CancellationToken cancellationToken);
        Task UpdateAsync(int id, RoleUpdateDto roleUpdateDto, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}