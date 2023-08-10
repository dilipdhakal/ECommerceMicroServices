using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Order.Application.Services.Implementation;
using Order.Application.Services.Interface;
using Order.Infrastructure.Repository;
using Order.Infrastructure.Repository.Implementation;
using Order.Infrastructure.Repository.Interface;

namespace OrderAPI.Configurations
{
    public static class ServiceConfigurationExtension
    {
        internal static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
          
            var t=configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<OrderDbContext>(options =>
                  options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement{{
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
                new string[]{}
                    }
                });
            });
            return services;

        }
    }
}
