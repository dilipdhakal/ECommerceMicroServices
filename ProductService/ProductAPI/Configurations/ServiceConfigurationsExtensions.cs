using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Product.Application.Services.Implementation;
using Product.Application.Services.Interface;
using Product.Infrastructure;
using Product.Infrastructure.Repository.Implementation;
using Product.Infrastructure.Repository.Interface;



namespace ProductAPI.Configurations
{
    public static class ServiceConfigurationsExtensions
    {
        internal static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options =>
                  options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductRepository,ProductRepository>();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
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
