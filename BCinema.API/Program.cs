using BCinema.API.Middlewares;
using BCinema.API.Responses;
using BCinema.Application;
using BCinema.Infrastructure;
using BCinema.Persistence;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using DependencyInjection = BCinema.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Set the environment variable for Google Application Default Credentials
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
    Path.Combine(AppContext.BaseDirectory, "firebase-adminsdk.json"));

// Initialize Firebase
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("firebase-adminsdk.json"),
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add other services
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errorMessage = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .Select(e => e.Value?.Errors.FirstOrDefault()?.ErrorMessage ?? "Unknown error")
                .FirstOrDefault() ?? "Unknown error";

            var response = new ApiResponse<object>(false, errorMessage);
            return new BadRequestObjectResult(response);
        };
    });


builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPersistence(builder.Configuration);

var app = builder.Build();

await DependencyInjection.InitializeDatabaseAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseMiddleware<DateValidationMiddleware>();
app.UseMiddleware<GlobalExceptionHandleMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();
