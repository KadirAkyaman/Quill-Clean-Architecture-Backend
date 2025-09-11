using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Quill.Application.DTOs.Tag;

namespace Quill.Application.Validators.Tag
{
    public class TagCreateDtoValidator : AbstractValidator<TagCreateDto>
    {
        public TagCreateDtoValidator()
        {
             RuleFor(t => t.Name)
                .NotEmpty().WithMessage("Tag name is required.")
                .Length(2, 25).WithMessage("Tag name must be between 2 and 25 characters.");
        }
    }
}