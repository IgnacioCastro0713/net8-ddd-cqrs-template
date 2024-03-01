using Api.Core.Setups;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiDependencyInjection(
	    this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

        services
            .AddHttpContextAccessor()
            .AddEndpointsApiExplorer()
            .AddAuthenticationConfiguration()
            .AddConfigureOptions()
            .AddCorsPolicy(configuration)
            .AddSwaggerGen();

        return services;
    }

    private static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
    {
	    services
		    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		    .AddJwtBearer();

        return services;
    }

    private static IServiceCollection AddConfigureOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        return services;
    }

    private static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(opt => opt.AddPolicy("_AllowedPolicies", policyBuilder =>
        {

            policyBuilder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }));

        return services;
    }

    private static IServiceCollection AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "API.Backend"
            });

            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }
}