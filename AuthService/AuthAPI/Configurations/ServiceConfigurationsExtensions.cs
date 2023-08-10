using Auth.Application.Services.Implementation;
using Auth.Application.Services.Interface;
using Auth.Infrastructure;
using Auth.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;



namespace AuthApi.Configurations
{
    public static class ServiceConfigurationsExtensions
    {
        internal static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            var t = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AuthDbContext>(options =>
                  options.UseNpgsql(t));
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<AuthRepository>();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
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
