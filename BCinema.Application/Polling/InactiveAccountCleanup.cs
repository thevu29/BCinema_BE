using BCinema.Domain.Interfaces.IRepositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BCinema.Application.Polling;

public class InactiveAccountCleanup(IServiceProvider serviceProvider, ILogger<InactiveAccountCleanup> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CleanupInactiveAccountsAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Run once a day
        }
    }
    
    private async Task CleanupInactiveAccountsAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        await userRepository.DeleteInActiveUsersAsync(cancellationToken);
        logger.LogInformation("Inactive accounts cleanup completed.");
    }
}