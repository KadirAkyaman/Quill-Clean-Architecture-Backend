using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Quill.Application.DTOs.User;

namespace Quill.Application.Validators.User
{
    public class UserUpdateProfileDtoValidator : AbstractValidator<UserUpdateProfileDto>
    {
        public UserUpdateProfileDtoValidator()
        {
            RuleFor(u => u.Name)
                .Length(2, 30).WithMessage("Name must be between 2 and 30 characters.")
                .When(u => !string.IsNullOrEmpty(u.Name));

            RuleFor(u => u.Surname)
                .Length(2, 30).WithMessage("Surname must be between 2 and 30 characters.")
                .When(u => !string.IsNullOrEmpty(u.Surname));

            RuleFor(u => u.Username)
                .Length(3, 15).WithMessage("Username must be between 3 and 15 characters.")
                .When(u => !string.IsNullOrEmpty(u.Username));
        }
    }
}