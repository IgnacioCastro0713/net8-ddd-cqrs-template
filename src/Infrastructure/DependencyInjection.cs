using Application.Abstractions.Services.Cache;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencyInjection(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddMemoryCache()
            .AddDistributedMemoryCache()
            .AddInfrastructureAssemblyScanning()
            .AddDbContextConfiguration(configuration);

        return services;
    }

    private static IServiceCollection AddInfrastructureAssemblyScanning(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, CacheService>();
        services.Scan(selector => selector
            .FromAssemblies(AssemblyReference.Assembly)
            .AddClasses(false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddDbContextConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            options
                .UseSqlServer(configuration.GetConnectionString("Connection"))
                .AddInterceptors(
                    provider.GetRequiredService<AuditableEntityInterceptor>(),
                    provider.GetRequiredService<SoftDeleteInterceptor>());
        });

        return services;
    }
}