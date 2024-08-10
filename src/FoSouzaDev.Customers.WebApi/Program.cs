using FoSouzaDev.Customers.Application.Settings;

namespace FoSouzaDev.Customers.WebApi;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.ClearProviders().AddConsole();

        builder.Services.AddApplicationServices(builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddExceptionHandler<ApplicationExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddHealthChecks().AddApplicationHealthChecks();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();
        app.UseExceptionHandler();

        app.UseHealthChecks("/api/health-check");

        app.Run();
    }
}