using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;
using Talabat_APIs.Errors;
using Talabat_APIs.Helpers;

namespace Talabat_APIs.Extensions
{
    public static class AppilcationServiceExtenstions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IGenericRepositort<>), typeof(GenericRepository<>)); // Register the generic repository for any entity type
            //builder.Services.AddAutoMapper(M=>M.AddProfile(new MappingProfiles())); // Register AutoMapper with the mapping profiles
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.Configure<ApiBehaviorOptions>(optins =>
            {
                // Disable the default model state validation behavior
                optins.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray(); // Get the error messages from the model state
                    var ValidationErrors = new ApiVialidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrors);
                };
            });
            Services.AddScoped<IBasketRepository, BasketRepository>(); // Register the BasketRepository for IBasketRepository interface
            Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Register the UnitOfWork for IUnitOfWork interface
            Services.AddScoped<IOrderService, OrderService>(); // Register the OrderService for IOrderService interface
            return Services;
        }
    }
}