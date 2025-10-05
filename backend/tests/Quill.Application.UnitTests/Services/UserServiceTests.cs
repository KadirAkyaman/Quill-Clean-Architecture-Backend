using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Quill.Application.DTOs.User;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Repositories;
using Quill.Application.Interfaces.Services;
using Quill.Application.Options;
using Quill.Application.Services;
using Quill.Domain.Entities;
using Xunit;

namespace Quill.Application.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IOptions<JwtOptions>> _jwtOptionsMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _authServiceMock = new Mock<IAuthService>();
            _jwtOptionsMock = new Mock<IOptions<JwtOptions>>();

            _unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.RoleRepository).Returns(_roleRepositoryMock.Object);

            var jwtOptions = new JwtOptions { Issuer = "TestIssuer", Audience = "TestAudience", SecretKey = "a_super_secret_key_that_is_long_enough", ExpiryInMinutes = 60 };
            _jwtOptionsMock.Setup(o => o.Value).Returns(jwtOptions);

            _userService = new UserService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _authServiceMock.Object,
                _jwtOptionsMock.Object);
        }

        // ---REGISTER------------------------------------------------------------------------------------------------
        [Fact]
        public async Task RegisterAsync_WithUniqueCredentials_ShouldCreateUserAndReturnAuthResponse()
        {
            // Given
            var userRegisterDto = new UserRegisterDto
            {
                Name = "testtest",
                Surname = "testtest",
                Email = "test@gmail.com",
                Username = "testtest",
                Password = "testtest",
                ConfirmPassword = "testtest"
            };

            var newUser = new User
            {
                Name = userRegisterDto.Name,
                Surname = userRegisterDto.Surname,
                Email = userRegisterDto.Email,
                Username = userRegisterDto.Username,
                IsActive = true
            };

            var authorRole = new Role { Id = 2, Name = "Author" };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(userRegisterDto.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(userRegisterDto.Username, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            _roleRepositoryMock.Setup(repo => repo.GetByNameAsync("Author", It.IsAny<CancellationToken>()))
                .ReturnsAsync(authorRole);

            _mapperMock.Setup(m => m.Map<User>(userRegisterDto))
                .Returns(newUser);

            _authServiceMock.Setup(a => a.HashPassword(userRegisterDto.Password))
                .Returns("a_very_secure_hashed_password");

            var expectedToken = "a_valid_jwt_token";
            _authServiceMock.Setup(a => a.GenerateJwtToken(It.IsAny<User>()))
                .Returns(expectedToken);

            // When
            var result = await _userService.RegisterAsync(userRegisterDto, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Token.Should().Be(expectedToken);
            result.Username.Should().Be(userRegisterDto.Username);
            result.Role.Should().Be(authorRole.Name);

            _userRepositoryMock.Verify(repo => repo.AddAsync(
                It.Is<User>(u =>
                    u.Username == userRegisterDto.Username &&
                    u.Email == userRegisterDto.Email &&
                    u.RoleId == authorRole.Id &&
                    u.PasswordHash == "a_very_secure_hashed_password"
                ),
                It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WhenEmailIsAlreadyTaken_ShouldThrowConflictException()
        {
            // Given
            var userRegisterDto = new UserRegisterDto
            {
                Email = "test@gmail.com"
            };

            var existingUser = new User();

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(userRegisterDto.Email, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            // When
            Func<Task> act = async () => await _userService.RegisterAsync(userRegisterDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ConflictException>();

            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_WhenUsernameIsAlreadyTaken_ShouldThrowConflictException()
        {
            // Given
            var userRegisterDto = new UserRegisterDto
            {
                Email = "new_email@example.com",
                Username = "existingUsername"
            };

            var existingUser = new User();

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(userRegisterDto.Email, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(userRegisterDto.Username, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            // When
            Func<Task> act = async () => await _userService.RegisterAsync(userRegisterDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ConflictException>();

            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // ---LOGIN---------------------------------------------------------------------------------------------------
        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnAuthResponse()
        {
            // Given
            var userLoginDto = new UserLoginDto
            {
                Email = "test@gmail.com",
                Password = "password"
            };

            var existingUser = new User
            {
                Id = 5,
                Email = "test@gmail.com",
                PasswordHash = "correct_password_hash",
                Role = new Role
                {
                    Id = 2,
                    Name = "Author"
                }
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(userLoginDto.Email, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            _authServiceMock.Setup(auth => auth.VerifyPassword(userLoginDto.Password, existingUser.PasswordHash)).Returns(true);

            var expectedToken = "a_valid_jwt_token";

            _authServiceMock.Setup(auth => auth.GenerateJwtToken(It.IsAny<User>())).Returns(expectedToken);

            // When
            var result = await _userService.LoginAsync(userLoginDto, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Token.Should().Be(expectedToken);
            result.Username.Should().Be(existingUser.Username);
            result.Role.Should().Be(existingUser.Role.Name);
        }

        [Fact]
        public async Task LoginAsync_WhenUserNotFound_ShouldThrowUnauthorizedActionException()
        {
            // Given
            var userLoginDto = new UserLoginDto
            {
                Email = "test@gmail.com",
                Password = "any_password"
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(userLoginDto.Email, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            // When
            Func<Task> act = async () => await _userService.LoginAsync(userLoginDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<UnauthorizedActionException>();

            _authServiceMock.Verify(auth => auth.GenerateJwtToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordIsIncorrect_ShouldThrowUnauthorizedActionException()
        {
            // Given
            var userLoginDto = new UserLoginDto
            {
                Email = "test@gmail.com",
                Password = "wrong_password"
            };

            var existingUser = new User
            {
                Id = 1,
                Email = userLoginDto.Email,
                PasswordHash = "correct_password_hash"
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(userLoginDto.Email, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            _authServiceMock.Setup(auth => auth.VerifyPassword(userLoginDto.Password, existingUser.PasswordHash)).Returns(false);

            // When
            Func<Task> act = async () => await _userService.LoginAsync(userLoginDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<UnauthorizedActionException>();

            _authServiceMock.Verify(auth => auth.GenerateJwtToken(It.IsAny<User>()), Times.Never);
        }

        // ---CHANGE PASSWORD-----------------------------------------------------------------------------------------
        [Fact]
        public async Task ChangePasswordAsync_WhenCurrentPasswordIsCorrect_ShouldUpdatePassword()
        {
            // Given
            var userId = 1;
            var currentPassword = "current_password_123";
            var newPassword = "new_strong_password_456";
            var newPasswordHash = "hashed_new_password";

            var changePasswordDto = new UserChangePasswordDto
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = newPassword
            };

            var existingUser = new User
            {
                Id = userId,
                PasswordHash = "hashed_current_password"
            };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            _authServiceMock.Setup(auth => auth.VerifyPassword(currentPassword, existingUser.PasswordHash)).Returns(true);

            _authServiceMock.Setup(auth => auth.HashPassword(newPassword)).Returns(newPasswordHash);

            // When
            await _userService.ChangePasswordAsync(userId, changePasswordDto, CancellationToken.None);

            // Then
            _userRepositoryMock.Verify(repo => repo.Update(It.Is<User>(u => u.Id == userId && u.PasswordHash == newPasswordHash)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenCurrentPasswordIsIncorrect_ShouldThrowUnauthorizedActionException()
        {
            var userId = 1;
            var currentWrongPassword = "current_password";
            var hashedPassword = "current_hashed_password";
            // Given
            var changePasswordDto = new UserChangePasswordDto
            {
                CurrentPassword = currentWrongPassword
            };

            var existingUser = new User
            {
                Id = userId,
                PasswordHash = hashedPassword
            };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            _authServiceMock.Setup(repo => repo.VerifyPassword(currentWrongPassword, hashedPassword)).Returns(false);

            // When
            Func<Task> act = async () => await _userService.ChangePasswordAsync(userId, changePasswordDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<UnauthorizedActionException>();

            _userRepositoryMock.Verify(repo => repo.Update(It.Is<User>(u => u.Id == userId)), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // ---UPDATE PROFILE-------------------------------------------------------------------------------------------
        [Fact]
        public async Task UpdateProfileAsync_WithValidData_ShouldUpdateProfile()
        {
            // Given
            var userId = 5;
            var username = "test_username";
            var newUsername = "new_username";
            var updateProfileDto = new UserUpdateProfileDto
            {
                Username = newUsername
            };

            var existingUser = new User
            {
                Id = userId,
                Username = username
            };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(newUsername, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            _mapperMock.Setup(m => m.Map(updateProfileDto, existingUser))
                .Callback<UserUpdateProfileDto, User>((dto, user) =>
                {
                    user.Username = dto.Username!;
                });

            // When
            await _userService.UpdateProfileAsync(userId, updateProfileDto, CancellationToken.None);

            // Then
            _userRepositoryMock.Verify(repo => repo.Update(It.Is<User>(u => u.Id == userId && u.Username == newUsername)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProfileAsync_WhenUsernameIsTakenByAnotherUser_ShouldThrowConflictException()
        {
            // Given
            var userIdToUpdate = 5;
            var newUsername = "test_username";

            var updateProfileDto = new UserUpdateProfileDto
            {
                Username = newUsername
            };

            var currentUser = new User
            {
                Id = userIdToUpdate,
                Username = "original_username"
            };

            var otherUserWithUsername = new User
            {
                Id = 99,
                Username = newUsername
            };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userIdToUpdate, It.IsAny<CancellationToken>())).ReturnsAsync(currentUser);

            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(newUsername, It.IsAny<CancellationToken>())).ReturnsAsync(otherUserWithUsername);

            // When
            Func<Task> act = async () => await _userService.UpdateProfileAsync(userIdToUpdate, updateProfileDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ConflictException>();

            _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // ---BY ADMIN-------------------------------------------------------------------------------------------------
        [Fact]
        public async Task UpdateUserByAdminAsync_WithValidData_ShouldUpdateUser()
        {
            // Given
            var userId = 5;
            var username = "test_username";
            var newUsername = "new_username";
            var updateProfileDto = new AdminUserUpdateDto
            {
                Username = newUsername
            };

            var existingUser = new User
            {
                Id = userId,
                Username = username
            };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(newUsername, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            _mapperMock.Setup(m => m.Map(updateProfileDto, existingUser))
                .Callback<AdminUserUpdateDto, User>((dto, user) =>
                {
                    user.Username = dto.Username!;
                });

            // When
            await _userService.UpdateUserByAdminAsync(userId, updateProfileDto, CancellationToken.None);

            // Then
            _userRepositoryMock.Verify(repo => repo.Update(It.Is<User>(u => u.Id == userId && u.Username == newUsername)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserByAdminAsync_WhenUsernameIsTakenByAnotherUser_ShouldThrowConflictException()
        {
            // Given
            var userIdToUpdate = 5;
            var newUsername = "test_username";

            var updateProfileDto = new AdminUserUpdateDto
            {
                Username = newUsername
            };

            var currentUser = new User
            {
                Id = userIdToUpdate,
                Username = "original_username"
            };

            var otherUserWithUsername = new User
            {
                Id = 99,
                Username = newUsername
            };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userIdToUpdate, It.IsAny<CancellationToken>())).ReturnsAsync(currentUser);

            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(newUsername, It.IsAny<CancellationToken>())).ReturnsAsync(otherUserWithUsername);

            // When
            Func<Task> act = async () => await _userService.UpdateUserByAdminAsync(userIdToUpdate, updateProfileDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ConflictException>();

            _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ChangeUserRoleByAdminAsync_WhenRoleExists_ShouldUpdateUserRole()
        {
            // Given
            var newRoleId = 2;
            var targetUserId = 5;

            var newRoleFromDb = new Role { Id = newRoleId };

            var changeRoleDto = new AdminUserChangeRoleDto { RoleId = newRoleId };

            var userToUpdate = new User
            {
                Id = targetUserId,
                RoleId = 3
            };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(targetUserId, It.IsAny<CancellationToken>())).ReturnsAsync(userToUpdate);
            _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(newRoleId, It.IsAny<CancellationToken>())).ReturnsAsync(newRoleFromDb);

            // When
            await _userService.ChangeUserRoleByAdminAsync(targetUserId, changeRoleDto, CancellationToken.None);

            // Then
            _userRepositoryMock.Verify(repo => repo.Update(It.Is<User>(u => u.RoleId == newRoleId && u.Id == targetUserId)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ChangeUserRoleByAdminAsync_WhenRoleNotFound_ShouldThrowBadRequestException()
        {
            // Given
            var newRoleId = 4;
            var targetUserId = 3;

            var userToUpdate = new User { Id = targetUserId, RoleId = 2 };

            var changeRoleDto = new AdminUserChangeRoleDto { RoleId = newRoleId };


            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(targetUserId, It.IsAny<CancellationToken>())).ReturnsAsync(userToUpdate);

            _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(newRoleId, It.IsAny<CancellationToken>())).ReturnsAsync((Role?)null);

            // When
            Func<Task> act = async () => await _userService.ChangeUserRoleByAdminAsync(targetUserId, changeRoleDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<BadRequestException>();

            _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUserByAdminAsync_WithActiveUser_ShouldSetUserAsInactive()
        {
            // Given
            var userId = 5;
            var existingUser = new User { Id = userId, IsActive = true };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            // When
            await _userService.DeleteUserByAdminAsync(userId, CancellationToken.None);

            // Then
            _userRepositoryMock.Verify(repo => repo.Update(It.Is<User>(u => u.Id == userId && u.IsActive == false)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUserByAdminAsync_WhenUserIsAlreadyInactive_ShouldRemainInactive()
        {
            // Given
            var userId = 1;
            var inactiveUser = new User { Id = userId, IsActive = false };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(inactiveUser);

            // When
            await _userService.DeleteUserByAdminAsync(userId, CancellationToken.None);

            // Then
            _userRepositoryMock.Verify(repo => repo.Update(It.Is<User>(u => u.Id == userId && u.IsActive == false)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }        
    }
}