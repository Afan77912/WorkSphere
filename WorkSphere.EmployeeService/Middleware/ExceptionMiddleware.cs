using System.Net;
using System.Text.Json;
using WorkSphere.EmployeeService.Common;
using WorkSphere.EmployeeService.Exceptions;

namespace WorkSphere.EmployeeService.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            int statusCode;
            string message;

            switch (exception)
            {
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;

                case BadRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>(
                false,
                statusCode,
                message,
                null
            );

            var jsonResponse = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}