using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MigrationService;
using Producer.Data;

var builder = Host.CreateApplicationBuilder(args);

const string postgresConnectionString = "Postgres";

var connectionString = builder.Configuration.GetConnectionString(postgresConnectionString)
                       ?? throw new InvalidOperationException(
                           $"'{postgresConnectionString}' connection string is missing.");

builder.Services.AddDbContext<BooksDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddHostedService<MigrationWorker>();

await builder.Build().RunAsync();