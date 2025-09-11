using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Quill.Application.DTOs.Category;

namespace Quill.Application.Validators.Category
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Category name cannot be empty when provided.")
                .Length(2, 40).WithMessage("Category name must be between 2 and 40 characters.")
                .When(c => c.Name != null); ;
        }
    }
}