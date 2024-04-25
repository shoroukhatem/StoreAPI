using Microsoft.AspNetCore.Mvc;
using Store.Repository.BasketRepository;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.HandlingResponses;
using Store.Service.Services.BasketService;
using Store.Service.Services.BasketService.DTO;
using Store.Service.Services.CacheService;
using Store.Service.Services.OrderService;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.DTOs;
using Store.Service.Services.TokenService;
using Store.Service.Services.UserService;

namespace Store.API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Add Services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(OrderProfile));

            //add middleware
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(model => model.Value.Errors.Count > 0)
                                                           .SelectMany(model => model.Value.Errors)
                                                           .Select(error => error.ErrorMessage)
                                                           .ToList();
                    var errorResponse = new ValidationErrorsResponce
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }
    }
}
