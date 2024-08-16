using FoSouzaDev.Customers.WebApi.Settings;

namespace FoSouzaDev.Customers.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.ClearProviders().AddConsole();

        builder.Services.AddApiServices(builder.Configuration);

        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddExceptionHandler<ApplicationExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddHealthChecks().AddApiHealthChecks();

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