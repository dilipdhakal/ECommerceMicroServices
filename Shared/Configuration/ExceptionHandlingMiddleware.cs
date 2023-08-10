using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Wrapper;
using System.Data.SqlClient;
using System.Net;
using System.Security;
using System.Text;
using System.Text.Json;

namespace Shared.Configuration
{
    /// <summary>
    /// The extension to handle Exceptions and convert them to corresponding status code.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        /// <summary>
        /// Initializes the new instance of <see cref="ExceptionHandlingMiddleware"/>.
        /// </summary>
        /// <param name="next">The request to handle</param>
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Invokes the middleware handler.
        /// </summary>
        /// <param name="context">The current http context.</param>
        /// <param name="logger">The logger</param>
        public async Task Invoke(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="context">The current http conteext.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logger">The logger instance.</param>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception,
                                                ILogger<ExceptionHandlingMiddleware> logger)
        {
            var statusCode = default(HttpStatusCode);
            string message = "";
            string errorNo = "";


            switch (exception)
            {
                case SecurityException security:
                    statusCode = HttpStatusCode.Forbidden;
                    message += "You shall not pass!";
                    break;

                case ArgumentNullException argumentNullException:
                    statusCode = HttpStatusCode.InternalServerError;
                    message += exception.Message;
                    break;

                case NullReferenceException nullReferenceException:
                    statusCode = HttpStatusCode.NotAcceptable;
                    message += "Invalid Data";
                    break;

                case DbUpdateConcurrencyException dbException:
                    statusCode = HttpStatusCode.NotFound;
                    message += "Data not Found";
                    break;

                case ArgumentException argumentInValid:
                    statusCode = HttpStatusCode.BadRequest;
                    message += argumentInValid.Message;
                    break;

                //case TaskCanceledException taskCanceled:
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message += "Something is not right at this side. Please, inform me on dhkaldilip@gmail.com";
                    break;
            }

            logger.LogError($"Exception(error): {exception.Message} - Inner: {exception.InnerException?.Message} - Stacktrace: {exception.StackTrace}");
            var response = new Response();
            response.Succeeded = false;
            response.Messages = message;

            var result = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(result, Encoding.UTF8);
        }
    }
}
