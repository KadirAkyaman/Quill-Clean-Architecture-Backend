using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Quill.Application.DTOs.Role;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Services;
using Quill.Domain.Entities;

namespace Quill.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoleDto> CreateAsync(RoleCreateDto roleCreateDto, CancellationToken cancellationToken)
        {
            var role = _mapper.Map<Role>(roleCreateDto);

            await _unitOfWork.RoleRepository.AddAsync(role, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RoleDto>(role);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var role = await GetRoleAndEnsureExists(id, cancellationToken);

            _unitOfWork.RoleRepository.Remove(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<RoleDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<IReadOnlyList<RoleDto>>(roles);
        }

        public async Task<RoleDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.RoleRepository.GetByNameAsync(name, cancellationToken);

            return _mapper.Map<RoleDto>(role);
        }

        public async Task UpdateAsync(int id, RoleUpdateDto roleUpdateDto, CancellationToken cancellationToken)
        {
            var role = await GetRoleAndEnsureExists(id, cancellationToken);

            _mapper.Map(roleUpdateDto, role);
            _unitOfWork.RoleRepository.Update(role); 
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }


        //HELPER
        private async Task<Role> GetRoleAndEnsureExists(int id, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id, cancellationToken);

            if (role is null)
                throw new NotFoundException($"Role with ID {id} not found.");

            return role;
        }
    }
}