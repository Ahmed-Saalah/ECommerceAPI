using eCommerceApp.Application.Mapping;
using eCommerceApp.Application.Services.Implemetations;
using eCommerceApp.Application.Services.Implemetations.Authentication;
using eCommerceApp.Application.Services.Interfaces;
using eCommerceApp.Application.Services.Interfaces.Authentication;
using eCommerceApp.Application.Validation;
using eCommerceApp.Application.Validation.Authentication;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceApp.Application.DependancyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService
            (this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingConfig));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryServic>();
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;    
        }
    }
}
