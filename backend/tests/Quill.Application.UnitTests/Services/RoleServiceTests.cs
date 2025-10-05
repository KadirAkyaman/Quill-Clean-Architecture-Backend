using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Quill.Application.DTOs.Role;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Repositories;
using Quill.Application.Services;
using Quill.Domain.Entities;
using Xunit;

namespace Quill.Application.UnitTests.Services
{
    public class RoleServiceTests
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RoleService _roleService;

        public RoleServiceTests()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.RoleRepository).Returns(_roleRepositoryMock.Object);

            _roleService = new RoleService(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        // ---CREATE------------------------------------------------------------------------------------------------------- 	

        [Fact]
        public async Task CreateAsync_WithUniqueName_ShouldCreateRole()
        {
            // Given
            var roleCreateDto = new RoleCreateDto { Name = "New Role" };
            var newRole = new Role { Name = roleCreateDto.Name };

            _roleRepositoryMock.Setup(repo => repo.GetByNameAsync(roleCreateDto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Role?)null);

            _mapperMock.Setup(m => m.Map<Role>(roleCreateDto)).Returns(newRole);
            _mapperMock.Setup(m => m.Map<RoleDto>(newRole)).Returns(new RoleDto());

            // When
            await _roleService.CreateAsync(roleCreateDto, CancellationToken.None);

            // Then
            _roleRepositoryMock.Verify(repo => repo.GetByNameAsync(roleCreateDto.Name, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.AddAsync(newRole, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenRoleNameExists_ShouldThrowConflictException()
        {
            // Given
            var roleCreateDto = new RoleCreateDto { Name = "Existing Role" };
            var existingRole = new Role { Id = 1, Name = roleCreateDto.Name };

            _roleRepositoryMock.Setup(repo => repo.GetByNameAsync(roleCreateDto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRole);

            // When
            Func<Task> act = async () => await _roleService.CreateAsync(roleCreateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ConflictException>().WithMessage("A role with this name already exists.");

            _roleRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Role>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // ---UPDATE------------------------------------------------------------------------------------------------------- 	

        [Fact]
        public async Task UpdateAsync_WhenRoleExistsAndNameIsUnique_ShouldUpdateRole()
        {
            // Given
            var roleId = 1;
            var roleUpdateDto = new RoleUpdateDto { Name = "Updated Name" };
            var roleFromDb = new Role { Id = roleId, Name = "Original Name" };

            _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(roleFromDb);

            _roleRepositoryMock.Setup(repo => repo.GetByNameAsync(roleUpdateDto.Name!, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Role?)null);

            _mapperMock.Setup(m => m.Map(roleUpdateDto, roleFromDb))
            .Callback<RoleUpdateDto, Role>((dto, role) => 
            {
                role.Name = dto.Name!;
            });

            // When
            await _roleService.UpdateAsync(roleId, roleUpdateDto, CancellationToken.None);

            // Then
            _roleRepositoryMock.Verify(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.Update(It.Is<Role>(r => r.Name == roleUpdateDto.Name)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenRoleNotFound_ShouldThrowNotFoundException()
        {
            // Given
            var roleId = 999;
            var roleUpdateDto = new RoleUpdateDto { Name = "Updated Name" };

            _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Role?)null);

            // When
            Func<Task> act = async () => await _roleService.UpdateAsync(roleId, roleUpdateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<NotFoundException>();
            
            _roleRepositoryMock.Verify(repo => repo.Update(It.IsAny<Role>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenNewNameIsTakenByAnotherRole_ShouldThrowConflictException()
        {
            // Given
            var roleIdToUpdate = 1;
            var newName = "Taken Role Name";
            var roleUpdateDto = new RoleUpdateDto { Name = newName };

            var roleFromDb = new Role { Id = roleIdToUpdate, Name = "Original Name" };
            var conflictingRole = new Role { Id = 2, Name = newName }; 

            _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(roleIdToUpdate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(roleFromDb);

            _roleRepositoryMock.Setup(repo => repo.GetByNameAsync(newName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(conflictingRole);

            // When
            Func<Task> act = async () => await _roleService.UpdateAsync(roleIdToUpdate, roleUpdateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ConflictException>().WithMessage("A role with this name already exists.");
            
            _roleRepositoryMock.Verify(repo => repo.Update(It.IsAny<Role>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // ---DELETE-------------------------------------------------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_WhenRoleExists_ShouldDeleteRole()
        {
            // Given
            var roleId = 1;
            var roleFromDb = new Role { Id = roleId };

            _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(roleFromDb);

            // When
            await _roleService.DeleteAsync(roleId, CancellationToken.None);

            // Then
            _roleRepositoryMock.Verify(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.Remove(roleFromDb), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenRoleNotFound_ShouldThrowNotFoundException()
        {
            // Given
            var roleId = 999;

            _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Role?)null);

            // When
            Func<Task> act = async () => await _roleService.DeleteAsync(roleId, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<NotFoundException>();
            
            _roleRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Role>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}