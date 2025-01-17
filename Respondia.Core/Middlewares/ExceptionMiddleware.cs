using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Respondia.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Respondia.Core.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (OperationCanceledException oce)
            {
                _logger.LogWarning(oce, "The operation was canceled.");

                context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "The request was canceled by the client or the server."
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning(vex, "Validation error occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "Validation errors occurred.",
                    errors = vex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (ServiceException sex)
            {
                _logger.LogError(sex, "A service error occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = sex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (DataAccessException dax)
            {
                _logger.LogError(dax, "A data access error occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = dax.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (PresentationException pex)
            {
                _logger.LogError(pex, "A presentation error occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = pex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "An unexpected error occurred. Please try again later."
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
