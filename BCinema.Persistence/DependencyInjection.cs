using BCinema.Application.Interfaces;
using BCinema.Persistence.Context;
using BCinema.Persistence.Seeds;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BCinema.Persistence
{
    public class DatabaseInitializer
    {
        // This class will be used for logging purposes
    }
    
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => 
                provider.GetRequiredService<ApplicationDbContext>());
            
            services.AddDataSeeders();

            return services;
        }

        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseInitializer>>();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

            try
            {
                var databaseExists = await context.Database.CanConnectAsync();

                if (!databaseExists)
                {
                    await context.Database.MigrateAsync();
                }
                else
                {
                    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                    if (pendingMigrations.Any())
                    {
                        await context.Database.MigrateAsync();
                    }
                }
                
                await dataSeeder.SeedAllAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }
        }
    }
}
