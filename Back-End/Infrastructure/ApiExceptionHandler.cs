using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Infrastructure;

public sealed class ApiExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<ApiExceptionHandler> _logger;

    public ApiExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<ApiExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int statusCode = exception switch
        {
            ValidationException or ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        if (statusCode >= 500)
        {
            _logger.LogError(exception, "Erro não tratado ao processar {Method} {Path}",
                httpContext.Request.Method,
                httpContext.Request.Path);
        }
        else
        {
            _logger.LogWarning(exception, "Requisição rejeitada em {Method} {Path}",
                httpContext.Request.Method,
                httpContext.Request.Path);
        }

        httpContext.Response.StatusCode = statusCode;
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = statusCode switch
            {
                StatusCodes.Status400BadRequest => "Dados inválidos",
                StatusCodes.Status404NotFound => "Recurso não encontrado",
                StatusCodes.Status409Conflict => "Conflito ao atualizar o recurso",
                _ => "Erro interno do servidor"
            },
            Detail = statusCode >= 500
                ? "Ocorreu um erro inesperado. Consulte os logs usando o identificador da requisição."
                : exception.Message,
            Instance = httpContext.Request.Path
        };
        problem.Extensions["traceId"] = httpContext.TraceIdentifier;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception
        });
    }
}
