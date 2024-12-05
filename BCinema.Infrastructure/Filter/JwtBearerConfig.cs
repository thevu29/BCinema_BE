using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BCinema.Infrastructure.Filter;

public static class JwtBearerConfig
{
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, string issuer, string audience, string secret)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    ValidateIssuerSigningKey = true,
                    
                };
            });

        return services;
    }
    
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtConfig");
        string issuer = jwtSettings["Issuer"]!;
        string audience = jwtSettings["Audience"]!;
        string secret = jwtSettings["Secret"]!;
        
        return services.AddJwtBearerAuthentication(issuer, audience, secret);
    }
}