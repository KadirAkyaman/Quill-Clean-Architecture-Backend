using Quill.Application.Exceptions;

namespace Quill.Presentation.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred: {Message}", ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = StatusCodes.Status500InternalServerError;
            string title = "An internal server error has occurred.";
            string detail = exception.Message;

            switch (exception)
            {
                case BadRequestException badRequestException:
                    statusCode = StatusCodes.Status400BadRequest;
                    title = "Bad Request";
                    detail = badRequestException.Message;
                    break;

                case UnauthorizedActionException unauthorizedActionException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    title = "Unauthorized";
                    detail = unauthorizedActionException.Message;
                    break;

                case NotFoundException notFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    title = "Not Found";
                    detail = notFoundException.Message;
                    break;

                case ConflictException conflictException:
                    statusCode = StatusCodes.Status409Conflict;
                    title = "Conflict";
                    detail = conflictException.Message;
                    break;

                case AlreadySubscribedException alreadySubscribedException:
                    statusCode = StatusCodes.Status409Conflict;
                    title = "Conflict";
                    detail = alreadySubscribedException.Message;
                    break;

                case InvalidSubscriptionOperationException invalidSubscriptionOperationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    title = "Bad Request";
                    detail = invalidSubscriptionOperationException.Message;
                    break;
            }

            context.Response.StatusCode = statusCode;

            var problemDetails = new
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                TraceId = System.Diagnostics.Activity.Current?.Id ?? context.TraceIdentifier
            };
            
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}