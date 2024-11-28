using BCinema.API.Responses;
using Newtonsoft.Json;
using System.Net;

namespace BCinema.API.Middlewares
{
    public class GlobalExceptionHandleMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandleMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ApiResponse<string>(false, exception.Message);

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }

}
