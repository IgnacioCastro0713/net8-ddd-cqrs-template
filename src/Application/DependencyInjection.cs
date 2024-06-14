using Application.Abstractions.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services)
    {
        services
            .AddApplicationAssemblyScanning()
            .AddValidatorsFromAssembly(AssemblyReference.Assembly)
            .AddMediatRConfiguration()
            .AddMappings();

        return services;
    }

    private static IServiceCollection AddApplicationAssemblyScanning(this IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromAssemblies(AssemblyReference.Assembly)
            .AddClasses(false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(QueryCachingBehavior<,>));
        });

        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(AssemblyReference.Assembly);

        services.AddSingleton(config);

        return services;
    }
}