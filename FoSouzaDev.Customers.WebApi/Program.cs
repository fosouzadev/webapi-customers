
using FoSouzaDev.Customers.WebApi.Domain.Repositories;
using FoSouzaDev.Customers.WebApi.Domain.Services;
using FoSouzaDev.Customers.WebApi.Infrastructure.Repositories;

namespace FoSouzaDev.Customers.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
        builder.Services.AddSingleton<ICustomerService, CustomerService>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddExceptionHandler<ApplicationExceptionHandler>();
        builder.Services.AddProblemDetails();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();
        app.UseExceptionHandler();

        app.Run();
    }
}