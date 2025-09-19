using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Quill.Application.DTOs.Category;
using Quill.Application.DTOs.Post;
using Quill.Application.DTOs.Role;
using Quill.Application.DTOs.Subscription;
using Quill.Application.DTOs.Tag;
using Quill.Application.DTOs.User;
using Quill.Domain.Entities;

namespace Quill.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Category
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();

            //Post
            CreateMap<Post, PostDto>();
            CreateMap<Post, PostPreviewDto>();
            CreateMap<PostCreateDto, Post>();
            CreateMap<PostUpdateDto, Post>();

            //Role
            CreateMap<Role, RoleDto>();
            CreateMap<RoleCreateDto, Role>();
            CreateMap<RoleUpdateDto, Role>();

            //Subscription
            CreateMap<Subscription, SubscriptionDto>();
            CreateMap<SubscriptionCreateDto, Subscription>();

            //Tag
            CreateMap<Tag, TagDto>();
            CreateMap<TagCreateDto, Tag>();
            CreateMap<TagUpdateDto, Tag>();

            //User
            CreateMap<User, UserDto>();
            CreateMap<User, UserProfileDto>();
            CreateMap<User, UserSummaryDto>();
            //AuthResponseDto
            //UserStatsDto
            CreateMap<AdminUserChangeRoleDto, User>();
            CreateMap<AdminUserUpdateDto, User>();
            CreateMap<UserChangePasswordDto, User>();
            CreateMap<UserLoginDto, User>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserUpdateProfileDto, User>();
        }
    }
}