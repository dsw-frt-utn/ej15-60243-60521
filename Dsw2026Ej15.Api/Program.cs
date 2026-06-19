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
            app.MapHealthChecks("/health-check"); //mapee en esa direccion
            
            //es para ver si esta funcionando el servidor
            //queremos saber porque es una API, con el objetivo que se conecte una app
            //cuando estan comunicandose apps en comun, primero hay que checkear si esta funcionando o no
            //si no obtiene una respuyesta y obtiene un timeout la ocmunicacion, en vez de seguir insistiendo:
            //opc1: endopoint que devuelva que esta activo, pero para que si el framw da el health-check
            
            // es un medio para que el cliente sepa si esta sanito o no

            app.Run();

            // ahora estamos con 3 capas: api, domain, data. enunciado: 1 capa aplicacion (leer con + atencion)
        }
    }
}
