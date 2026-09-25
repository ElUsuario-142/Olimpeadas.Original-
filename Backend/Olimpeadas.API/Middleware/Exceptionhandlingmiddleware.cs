using Microsoft.AspNetCore.Mvc;

namespace Olimpeadas.API.Middleware
{
    /* <summary>
     Traduce las excepciones que lanzan los servicios a respuestas HTTP con formato
     ProblemDetails (RFC 9457). Así los controllers no repiten try/catch y el cliente
     (React) recibe siempre la misma estructura de error
     Se registra despues de UseCors en Program.cs: como no limpiamos la respuesta,
     los headers de CORS se conservan y el navegador puede leer el mensaje de error.
     </summary> */
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex) when (!context.Response.HasStarted)
            {
                var (status, title) = ex switch
                {
                    KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
                    UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "No autorizado"),
                    InvalidOperationException => (StatusCodes.Status400BadRequest, "Solicitud inválida"),
                    _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
                };

                if (status == StatusCodes.Status500InternalServerError)
                    _logger.LogError(ex, "Excepción no controlada en {Method} {Path}",
                        context.Request.Method, context.Request.Path);
                else
                    _logger.LogWarning("{Method} {Path} -> {Status}: {Mensaje}",
                        context.Request.Method, context.Request.Path, status, ex.Message);

                var problem = new ProblemDetails
                {
                    Status = status,
                    Title = title,
                    // En los 500 NUNCA se devuelve ex.Message, ya que puede filtrar detalles internos  como SQL, rutas, etc
                    Detail = status == StatusCodes.Status500InternalServerError
                        ? "Ocurrió un error inesperado. Intentá nuevamente más tarde."
                        : ex.Message,
                    Instance = context.Request.Path.Value
                };
                problem.Extensions["traceId"] = context.TraceIdentifier;

                context.Response.StatusCode = status;
                await context.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json");
            }
        }
    }
}
