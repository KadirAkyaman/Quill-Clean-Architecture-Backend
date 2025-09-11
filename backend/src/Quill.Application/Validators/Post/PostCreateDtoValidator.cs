using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Quill.Application.DTOs.Post;

namespace Quill.Application.Validators.Post
{
    public class PostCreateDtoValidator : AbstractValidator<PostCreateDto>
    {
        public PostCreateDtoValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(5).WithMessage("Title must be at least 5 characters long.")
                .MaximumLength(200).WithMessage("Title cannot be longer than 200 characters.");

            RuleFor(p => p.Text)
                .NotEmpty().WithMessage("Post content is required.")
                .MinimumLength(50).WithMessage("Post content must be at least 50 characters long.");

            RuleFor(p => p.Summary)
                .NotEmpty().WithMessage("Summary is required.")
                .MinimumLength(10).WithMessage("Summary must be at least 10 characters long.")
                .MaximumLength(300).WithMessage("Summary cannot be longer than 300 characters.");

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("A valid category must be selected.");

            RuleForEach(p => p.TagIds)
                .GreaterThan(0).WithMessage("Each Tag ID must be a valid positive number.");
        }
    }
}