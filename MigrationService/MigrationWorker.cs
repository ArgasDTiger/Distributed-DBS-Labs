using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Producer.Data;

namespace MigrationService;

internal sealed class MigrationWorker(IServiceProvider services, IHostApplicationLifetime lifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BooksDbContext>();

        var strategy = db.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(() => db.Database.MigrateAsync(cancellationToken));

        lifetime.StopApplication();
    }
}