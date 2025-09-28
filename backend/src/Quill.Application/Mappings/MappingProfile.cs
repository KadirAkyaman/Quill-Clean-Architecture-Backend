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
            CreateMap<Subscription, SubscriptionDto>()
            .ForMember(dest => dest.SubscriptionDate,
               opt => opt.MapFrom(src => (src.IsActive && src.UpdatedAt.HasValue) ? src.UpdatedAt.Value : src.CreatedAt));

            CreateMap<SubscriptionCreateDto, Subscription>();

            //Tag
            CreateMap<Tag, TagDto>();
            CreateMap<TagCreateDto, Tag>();
            CreateMap<TagUpdateDto, Tag>();

            //User
            CreateMap<User, UserDto>()
            .ForMember(dest => dest.MemberSince, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.MemberSince, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<User, UserSummaryDto>();

            CreateMap<AdminUserChangeRoleDto, User>();

            CreateMap<AdminUserUpdateDto, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            CreateMap<UserRegisterDto, User>();

            CreateMap<UserUpdateProfileDto, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}