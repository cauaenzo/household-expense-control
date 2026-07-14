using System.Text.Json;

namespace GastosResidenciais.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso nao encontrado.");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";
            var corpo = JsonSerializer.Serialize(new { mensagem = ex.Message });
            await context.Response.WriteAsync(corpo);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operacao invalida.");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            var corpo = JsonSerializer.Serialize(new { mensagem = ex.Message });
            await context.Response.WriteAsync(corpo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno nao tratado.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var corpo = JsonSerializer.Serialize(new { mensagem = "Ocorreu um erro interno. Tente novamente." });
            await context.Response.WriteAsync(corpo);
        }
    }
}
