using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Quill.Application.DTOs.User;

namespace Quill.Application.Validators.User
{
    public class UserChangePasswordDtoValidator : AbstractValidator<UserChangePasswordDto>
    {
        public UserChangePasswordDtoValidator()
        {
            RuleFor(u => u.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.");

            RuleFor(u => u.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("New password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("New password must contain at least one number.")
                .NotEqual(u => u.CurrentPassword).WithMessage("New password cannot be the same as the current password.");

            RuleFor(u => u.ConfirmNewPassword)
                .NotEmpty().WithMessage("New password confirmation is required.")
                .Equal(u => u.NewPassword).WithMessage("New passwords do not match.");
        }
    }
}