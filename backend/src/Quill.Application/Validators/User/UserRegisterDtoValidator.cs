using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Quill.Application.DTOs.User;

namespace Quill.Application.Validators.User
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 30).WithMessage("Name must be between 2 and 30 characters.");

            RuleFor(u => u.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .Length(2, 30).WithMessage("Surname must be between 2 and 30 characters.");

            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 15).WithMessage("Username must be between 3 and 15 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(30).WithMessage("Email cannot be longer than 30 characters.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.");

            RuleFor(u => u.ConfirmPassword)
                .NotEmpty().WithMessage("Password confirmation is required.")
                .Equal(u => u.Password).WithMessage("Passwords do not match.");
        }
    }
}