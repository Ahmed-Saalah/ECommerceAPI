using eCommerceApp.Application.DTOs;
using FluentValidation;

namespace eCommerceApp.Application.Validation
{
    public class ValidationService : IValidationService
    {
        public async Task<ServiceResponse> ValidateAsync<T1>(T1 model, IValidator<T1> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                string errorsToString = string.Join(", ", errors);
                return new ServiceResponse { Message = errorsToString };
            }
            return new ServiceResponse { Success = true };
        }
    }
}
