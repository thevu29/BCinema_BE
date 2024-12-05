using BCinema.API.Middlewares;
using BCinema.API.Responses;
using BCinema.Application;
using BCinema.Application.Helpers;
using BCinema.Application.Mail;
using BCinema.Application.Momo;
using BCinema.Application.Polling;
using BCinema.Infrastructure;
using BCinema.Infrastructure.Filter;
using BCinema.Persistence;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using DependencyInjection = BCinema.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<InactiveAccountCleanup>();

builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));

// Set the environment variable for Google Application Default Credentials
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
    Path.Combine(AppContext.BaseDirectory, "firebase-adminsdk.json"));

// Initialize Firebase
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("firebase-adminsdk.json"),
});

builder.Services.AddJwtBearerAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("http://localhost:5173");
        corsPolicyBuilder.AllowAnyMethod();
        corsPolicyBuilder.AllowAnyHeader();
        corsPolicyBuilder.AllowCredentials();
    });
});

// Register JwtProvider
builder.Services.AddSingleton<JwtProvider>();

builder.Services.AddHttpContextAccessor();
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

builder.Services.AddTransient<IMailService, MailService>();

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

app.UseMiddleware<GlobalExceptionHandleMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
