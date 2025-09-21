using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Application.DTOs.User;

namespace Quill.Application.Interfaces.Services
{
    public interface IUserService
    {
        //Authentication
        Task<AuthResponseDto> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken);
        Task<AuthResponseDto> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken);

        //Basic User Operations
        Task<IReadOnlyList<UserSummaryDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserProfileDto?> GetByUsernameAsync(string username, CancellationToken cancellationToken);

        //Profile Management (For Logged-In Users)
        Task UpdateProfileAsync(int userId, UserUpdateProfileDto userUpdateProfileDto, CancellationToken cancellationToken);
        Task ChangePasswordAsync(int userId, UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken);

        //Administrator Operations
        Task ChangeUserRoleByAdminAsync(int userId, AdminUserChangeRoleDto adminUserChangeRoleDto, CancellationToken cancellationToken);
        Task UpdateUserByAdminAsync(int userId, AdminUserUpdateDto adminUserUpdateDto, CancellationToken cancellationToken);
        Task DeleteUserByAdminAsync(int userId, CancellationToken cancellationToken);
        /*
        GetByEmailAsync is a critical method for the system's internal operations. 
        Service methods such as LoginAsync and RegisterAsync must use this repository method in the 
        background to check whether a user exists. Exposing an endpoint that 
        allows direct user lookup by email address to the outside world (API) 
        is generally undesirable and may pose a security/privacy risk.
        */
    }
}