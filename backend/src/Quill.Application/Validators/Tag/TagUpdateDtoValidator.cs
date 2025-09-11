using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Quill.Application.DTOs.Tag;

namespace Quill.Application.Validators.Tag
{
    public class TagUpdateDtoValidator : AbstractValidator<TagUpdateDto>
    {
        public TagUpdateDtoValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("Tag name cannot be empty when provided.")
                .Length(2, 25).WithMessage("Tag name must be between 2 and 25 characters.")
                .When(t => t.Name != null);
        }
    }
}