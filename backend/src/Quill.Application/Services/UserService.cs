using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Quill.Application.DTOs.Post;
using Quill.Application.DTOs.User;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Services;
using Quill.Application.Options;
using Quill.Domain.Entities;

namespace Quill.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly JwtOptions _jwtOptions;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService, IOptions<JwtOptions> jwtOptions)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task ChangePasswordAsync(int userId, UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken)
        {
            var user = await GetUserAndEnsureExists(userId, cancellationToken);

            var hashedPassword = user.PasswordHash;

            if (!_authService.VerifyPassword(userChangePasswordDto.CurrentPassword, hashedPassword)) //Current Password != Hashed Password
                throw new UnauthorizedActionException("Incorrect current password provided.");
        
            var newPasswordHash  = _authService.HashPassword(userChangePasswordDto.NewPassword);
            user.PasswordHash = newPasswordHash ;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);      
        }

        public async Task ChangeUserRoleByAdminAsync(int userId, AdminUserChangeRoleDto adminUserChangeRoleDto, CancellationToken cancellationToken)
        {
            var user = await GetUserAndEnsureExists(userId, cancellationToken);

            var roleExists = await _unitOfWork.RoleRepository.GetByIdAsync(adminUserChangeRoleDto.RoleId, cancellationToken);

            if (roleExists is null)
                throw new BadRequestException($"Role with ID {adminUserChangeRoleDto.RoleId} does not exist.");

            user.RoleId = adminUserChangeRoleDto.RoleId;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUserByAdminAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await GetUserAndEnsureExists(userId, cancellationToken);

            user.IsActive = false;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<UserSummaryDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<IReadOnlyList<UserSummaryDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserProfileDto?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByUsernameAsync(username, cancellationToken);

            if (user is null)
                return null;

            var userProfileDto = _mapper.Map<UserProfileDto>(user);

            var postCount = await _unitOfWork.PostRepository.GetCountByAuthorIdAsync(user.Id, cancellationToken);
            var subscriberCount = await _unitOfWork.SubscriptionRepository.GetSubscriberCountAsync(user.Id, cancellationToken);
            var subscriptionCount = await _unitOfWork.SubscriptionRepository.GetSubscriptionCountAsync(user.Id, cancellationToken);
            var recentPosts = await _unitOfWork.PostRepository.GetRecentByAuthorIdAsync(user.Id, 3, cancellationToken);

            userProfileDto.Stats = new UserStatsDto
            {
                PostsCount = postCount,
                SubscriberCount = subscriberCount,
                SubscriptionsCount = subscriptionCount
            };

            userProfileDto.RecentPosts = _mapper.Map<ICollection<PostPreviewDto>>(recentPosts);

            return userProfileDto;
        }

        public async Task<AuthResponseDto> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(userLoginDto.Email, cancellationToken);

            if (user is null || !_authService.VerifyPassword(userLoginDto.Password, user.PasswordHash))
                throw new UnauthorizedActionException("Invalid credentials provided. Please check your email and password.");
    
            var token = _authService.GenerateJwtToken(user);

            var expirationDate = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryInMinutes);

            var authResponseDto = new AuthResponseDto
            {
                Id = user.Id,
                Token = token,
                Expiration = expirationDate,
                Username = user.Username,
                Role = user.Role.Name
            };

            return authResponseDto;
        }

        public async Task<AuthResponseDto> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken)
        {
            var existingUserByEmail = await _unitOfWork.UserRepository.GetByEmailAsync(userRegisterDto.Email, cancellationToken);
            if (existingUserByEmail is not null)
            {
                throw new ConflictException("An account with this email address already exists.");
            }

            var existingUserByUsername = await _unitOfWork.UserRepository.GetByUsernameAsync(userRegisterDto.Username, cancellationToken);
            if (existingUserByUsername is not null)
            {
                throw new ConflictException("This username is already taken. Please choose another one.");
            }

            var newUser = _mapper.Map<User>(userRegisterDto);

            newUser.PasswordHash = _authService.HashPassword(userRegisterDto.Password);
            
            var defaultRole = await _unitOfWork.RoleRepository.GetByNameAsync("Author", cancellationToken);
            if (defaultRole is null)
            {
                throw new Exception("Default 'Author' role not found. Please configure the database.");
            }
            newUser.RoleId = defaultRole.Id;
            newUser.Role = defaultRole; 

            await _unitOfWork.UserRepository.AddAsync(newUser, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var token = _authService.GenerateJwtToken(newUser);
            var expirationDate = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryInMinutes);

            var authResponseDto = new AuthResponseDto
            {
                Id = newUser.Id,
                Token = token,
                Expiration = expirationDate,
                Username = newUser.Username,
                Role = newUser.Role.Name
            };

            return authResponseDto;
        }

        public async Task UpdateProfileAsync(int userId, UserUpdateProfileDto userUpdateProfileDto, CancellationToken cancellationToken)
        {
            var user = await GetUserAndEnsureExists(userId, cancellationToken);

            if (!string.IsNullOrWhiteSpace(userUpdateProfileDto.Username) && user.Username != userUpdateProfileDto.Username)
            {
                var existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(userUpdateProfileDto.Username, cancellationToken);
                if (existingUser is not null && existingUser.Id != userId)
                {
                    throw new ConflictException("This username is already taken by another user.");
                }
            }

            _mapper.Map(userUpdateProfileDto, user);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserByAdminAsync(int userId, AdminUserUpdateDto adminUserUpdateDto, CancellationToken cancellationToken)
        {
            var user = await GetUserAndEnsureExists(userId, cancellationToken);

            if (!string.IsNullOrWhiteSpace(adminUserUpdateDto.Username) && user.Username != adminUserUpdateDto.Username)
            {
                var existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(adminUserUpdateDto.Username, cancellationToken);
                if (existingUser is not null && existingUser.Id != userId)
                    throw new ConflictException("This username is already taken by another user.");
            }

            _mapper.Map(adminUserUpdateDto, user);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<User> GetUserAndEnsureExists(int userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
                throw new NotFoundException($"User with ID {userId} cannot found. ");

            return user;
        }
    }
}