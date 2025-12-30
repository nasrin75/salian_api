using Microsoft.AspNetCore.Diagnostics;
using salian_api.Models;
using System.Net;

namespace salian_api.Middleware
{   
    public class ErrorMessageHandler : IExceptionHandler
    {
        private readonly ILogger<ErrorMessageHandler> _logger;

        public ErrorMessageHandler(ILogger<ErrorMessageHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                $"An error occurred while processing your request: {exception.Message}");

            var errorResponse = new ErrorResponse
            {
                Message = exception.Message
            };

            switch (exception)
            {
                case BadHttpRequestException:
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            httpContext.Response.StatusCode = errorResponse.StatusCode;

            await httpContext
                .Response
                .WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
