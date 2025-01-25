using eCommerceApp.Application.DTOs.Identity;
using FluentValidation;

namespace eCommerceApp.Application.Validation.Authentication
{
    public class LoginUserValidator : AbstractValidator<LoginUser>
    {
        public LoginUserValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Enail is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}