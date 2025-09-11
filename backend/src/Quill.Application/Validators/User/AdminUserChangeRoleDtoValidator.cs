using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Quill.Application.DTOs.User;

namespace Quill.Application.Validators.User
{
    public class AdminUserChangeRoleDtoValidator : AbstractValidator<AdminUserChangeRoleDto>
    {
        public AdminUserChangeRoleDtoValidator()
        {
            RuleFor(u => u.RoleId)
                .GreaterThan(0).WithMessage("A valid Role ID is required.");
        }
    }
}