using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Quill.Application.DTOs.Post;

namespace Quill.Application.Validators.Post
{
    public class PostUpdateDtoValidator : AbstractValidator<PostUpdateDto>
    {
        public PostUpdateDtoValidator()
        {
            RuleFor(p => p.Title)
                .MinimumLength(5).WithMessage("Title must be at least 5 characters long.")
                .MaximumLength(200).WithMessage("Title cannot be longer than 200 characters.")
                .When(p => !string.IsNullOrEmpty(p.Title));

            RuleFor(p => p.Text)
                .MinimumLength(50).WithMessage("Post content must be at least 50 characters long.")
                .When(p => !string.IsNullOrEmpty(p.Text));

            RuleFor(p => p.Summary)
                .MinimumLength(10).WithMessage("Summary must be at least 10 characters long.")
                .MaximumLength(300).WithMessage("Summary cannot be longer than 300 characters.")
                .When(p => !string.IsNullOrEmpty(p.Summary));

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("A valid category ID must be provided.")
                .When(p => p.CategoryId.HasValue);

            RuleFor(p => p.Status)
                .IsInEnum().WithMessage("A valid post status must be selected.")
                .When(p => p.Status.HasValue);
            
            RuleForEach(p => p.TagIds)
                .GreaterThan(0).WithMessage("Each Tag ID must be a valid positive number.")
                .When(p => p.TagIds != null);
        }
    }
}