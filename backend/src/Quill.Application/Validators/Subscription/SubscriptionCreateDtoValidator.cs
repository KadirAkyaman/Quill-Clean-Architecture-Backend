using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Quill.Application.DTOs.Subscription;

namespace Quill.Application.Validators.Subscription
{
    public class SubscriptionCreateDtoValidator : AbstractValidator<SubscriptionCreateDto>
    {
        public SubscriptionCreateDtoValidator()
        {
            RuleFor(s => s.SubscribedToId)
                .GreaterThan(0).WithMessage("A valid user ID to subscribe to is required.");
        }
    }
}