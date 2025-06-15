using System.Net;
using System.Text.Json;
using HRManagement.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // continúa ejecución
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción atrapada por el middleware global");

            var response = new ErrorResponse();
            var statusCode = HttpStatusCode.InternalServerError;

            switch (ex)
            {
                case DbUpdateException dbEx
                    when dbEx.InnerException?.Message.Contains("IX_Employees_Email") == true:
                    statusCode = HttpStatusCode.Conflict; // 409
                    response.Message = "El correo ya existe.";
                    break;

                case DbUpdateException dbEx:
                    statusCode = HttpStatusCode.BadRequest;
                    response.Message = "Error al guardar en la base de datos.";
                    response.Details = dbEx.InnerException?.Message;
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response.Message = "El recurso solicitado no fue encontrado.";
                    break;

                case InvalidOperationException when ex.Message.Contains("Sequence contains no elements"):
                    statusCode = HttpStatusCode.NotFound;
                    response.Message = "El elemento no existe.";
                    break;

                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest;
                    response.Message = argEx.Message;
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    response.Message = "No autorizado.";
                    break;

                //  excepciones personalizadas...

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Ocurrió un error inesperado.";
                    response.Details = ex.Message;
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            response.Status = context.Response.StatusCode;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(result);
        }
    }
}
