using Api;
using Api.Core.Middlewares;
using Application;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    var configuration = builder.Configuration;
    var services = builder.Services;

    services
        .AddApiDependencyInjection(configuration)
        .AddApplicationDependencyInjection()
        .AddInfrastructureDependencyInjection(configuration);

    builder.Host.UseSerilog((context, loggerConfiguration) =>
        loggerConfiguration.ReadFrom.Configuration(context.Configuration));
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            opt.DefaultModelsExpandDepth(-1);
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors("_AllowedPolicies");
    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseSerilogRequestLogging();
    app.MapControllers();
}

app.Run();