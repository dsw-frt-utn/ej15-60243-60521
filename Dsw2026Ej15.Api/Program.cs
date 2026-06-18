using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Api.Middlewares;

namespace Dsw2026Ej15.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
            builder.Services.AddSwaggerGen();
                     //   builder.Services.AddOpenApi();
            builder.Services.AddSingleton<IPersistence, PersistenceInMemory>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
              //  app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers();
            app.MapHealthChecks("/health-check");

            app.Run();
        }
    }
}
