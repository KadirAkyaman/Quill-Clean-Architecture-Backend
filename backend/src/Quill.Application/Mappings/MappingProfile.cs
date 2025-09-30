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
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.PostCount, opt => opt.MapFrom<CategoryPostCountResolver>());
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();

            //Post
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(pt => pt.Tag)));
            CreateMap<Post, PostPreviewDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User)) 
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(pt => pt.Tag)));         

            CreateMap<PostCreateDto, Post>();
            CreateMap<PostUpdateDto, Post>()
                .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
                .ForMember(dest => dest.Text, opt => opt.Condition(src => src.Text != null))
                .ForMember(dest => dest.Summary, opt => opt.Condition(src => src.Summary != null))
                .ForMember(dest => dest.CategoryId, opt => opt.Condition(src => src.CategoryId.HasValue))
                .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status.HasValue))
                .ForMember(dest => dest.Tags, opt => opt.Ignore());

            //Role
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count));
            CreateMap<RoleCreateDto, Role>();
            CreateMap<RoleUpdateDto, Role>();

            //Subscription
            CreateMap<Subscription, SubscriptionDto>()
                .ForMember(dest => dest.SubscriptionDate, opt => opt.MapFrom(src => (src.IsActive && src.UpdatedAt.HasValue) ? src.UpdatedAt.Value : src.CreatedAt));

            CreateMap<SubscriptionCreateDto, Subscription>();

            //Tag
            CreateMap<Tag, TagDto>()
                .ForMember(dest => dest.PostCount, opt => opt.MapFrom<TagPostCountResolver>());
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