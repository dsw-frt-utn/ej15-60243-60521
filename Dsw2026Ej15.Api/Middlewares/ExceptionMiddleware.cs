using Dsw2026Ej15.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Dsw2026Ej15.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next; // es un deleado que pasa de esa manera el siguiente middleware el mensaje
    }

    // en la ejecucion un middle es un objetio proveniente de una clase, recibe, valida y pasa
    //      3 cosas que tiene que ser capaz un middleware
    // 1. un middleware tiene que ser capaz de recibir el mensaje que viene del middleware anterior
    // 2. aplicar una logica al mensaje para validar, porque un middleware tiene un objetivo 
    // 3. enviarle el mensaje al siguiente middleware
    // recibe mensaje -> aplica logica -> sigue la secuencia middleware

    // crear una clase que termine en middleware

    public async Task InvokeAsync(HttpContext context) //httpcontext es el mensaje que se va a pasar 
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        HttpStatusCode status = HttpStatusCode.InternalServerError;
        string message = "ocurrio un error inesperado al ejecutar la solicitud";
        if (ex is ValidationException ve)
        {
            message = ve.Message;
            status = HttpStatusCode.BadRequest;
        }

        var result = JsonSerializer.Serialize(new { error = message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        await context.Response.WriteAsync(result);
    }

    }
