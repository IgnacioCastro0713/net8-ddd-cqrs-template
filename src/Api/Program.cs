using Api.Middlewares;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    var configuration = builder.Configuration;
    var services = builder.Services;

    services.AddControllers();
    services
        .Scan(selector => selector
            .FromAssemblies(
                Infrastructure.AssemblyReference.Assembly,
                Application.AssemblyReference.Assembly
            )
            .AddClasses(false)
            .AsMatchingInterface()
            .WithScopedLifetime())
        .AddInfrastructure(configuration)
        .AddApplication()
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .AddExceptionHandler<GlobalExceptionHandler>()
        .AddProblemDetails();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt => opt.DefaultModelsExpandDepth(-1));
    }
    
    app.UseExceptionHandler()
        .UseHttpsRedirection()
        .UseAuthorization();
    

    app.MapControllers();
}

app.Run();