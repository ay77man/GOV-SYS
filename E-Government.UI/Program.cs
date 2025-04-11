using E_Government.Application;
using E_Government.Core.Domain.RepositoryContracts.Persistence;
using E_Government.Infrastructure;
using E_Government.Infrastructure._Data;
using E_Government.Infrastructure.UnitOfWork;
using E_Government.UI.Extension;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace E_Government.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
        

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Application layers
            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration);

            // Register services
            builder.Services.AddScoped<IDbInitializer, ApplicationDbInitializer>();
            builder.Services.AddDbContext<AccountDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("identity"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                 app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Government API v1");
                });

                app.UseCors("AllowAll");
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            await app.InitializDbAsync();
            app.Run();
        }
    }
}