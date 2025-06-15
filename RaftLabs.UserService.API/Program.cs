using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using UserServiceLibrary.Configuration;
using UserServiceLibrary.Interfaces;
using UserServiceLibrary.Services;
using Microsoft.Extensions.Options;

namespace RaftLabs.UserService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Bind ApiSettings from appsettings.json
            builder.Services.Configure<ApiSettings>(
                builder.Configuration.GetSection("ApiSettings"));

            // Register HttpClient for ExternalUserService using IHttpClientFactory
            builder.Services.AddHttpClient<IExternalUserService, ExternalUserService>((serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
                c.RoutePrefix = string.Empty; // Swagger at root: https://localhost:5000/
            });

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
