using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;

namespace BCinema.API.Middlewares
{
    public class DateValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DateValidationMiddleware> _logger;

        public DateValidationMiddleware(RequestDelegate next, ILogger<DateValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            if (context.Request.ContentType != null && 
                context.Request.ContentType.Contains("application/json"))
            {
                var bodyAsText = await new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    true,
                    1024,
                    true).ReadToEndAsync();

                context.Request.Body.Position = 0;

                if (!string.IsNullOrWhiteSpace(bodyAsText))
                {
                    var json = JObject.Parse(bodyAsText);
                    var dateFields = new List<string> { "expireAt", "date" };
                    var invalidDates = new List<string>();

                    foreach (var field in dateFields)
                    {
                        if (json[field] != null && json[field]?.Type == JTokenType.String)
                        {
                            var dateValue = json[field]?.ToString();

                            if (!DateTime.TryParseExact(
                                dateValue,
                                "yyyy-MM-dd",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out _))
                            {
                                invalidDates.Add(field);
                            }
                        }
                    }

                    if (invalidDates.Any())
                    {
                        var response = new
                        {
                            success = false,
                            message = "Invalid date format",
                            errors = invalidDates.Select(
                                d => $"{d} must be a valid date in format yyyy-MM-dd")
                        };

                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(response);
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
