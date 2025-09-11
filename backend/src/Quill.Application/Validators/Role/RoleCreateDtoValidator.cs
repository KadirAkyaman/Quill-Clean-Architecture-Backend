using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Quill.Application.DTOs.Role;

namespace Quill.Application.Validators.Role
{
    public class RoleCreateDtoValidator : AbstractValidator<RoleCreateDto>
    {
        public RoleCreateDtoValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Role name is required.")
                .Length(2, 20).WithMessage("Role name must be between 2 and 20 characters."); 
        }
    }
}