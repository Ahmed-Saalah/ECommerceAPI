using eCommerceApp.Application.DTOs;
using FluentValidation;

namespace eCommerceApp.Application.Validation
{
    public interface IValidationService
    {
        Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator);
    }
}
