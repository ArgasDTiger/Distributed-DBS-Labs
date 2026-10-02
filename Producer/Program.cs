using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Producer.Configurations;
using Producer.Data;
using Producer.Kafka;

const string postgresConnectionName = "Postgres";
const string kafkaConnectionName = "kafka";

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddOptions<ProducerConfiguration>()
    .BindConfiguration(nameof(ProducerConfiguration))
    .ValidateDataAnnotations()
    .ValidateOnStart();

string connectionString = builder.Configuration.GetConnectionString(postgresConnectionName)
                          ?? throw new InvalidOperationException($"'{postgresConnectionName}' connection string is missing.");

string kafkaBootstrapServers = builder.Configuration.GetConnectionString(kafkaConnectionName)
                               ?? throw new InvalidOperationException($"'{kafkaConnectionName}' connection string is missing.");

builder.Services.AddDbContextFactory<BooksDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddSingleton<BookPublisher>(sp => new BookPublisher(kafkaBootstrapServers, sp.GetRequiredService<IOptions<ProducerConfiguration>>()));

builder.Services.AddHostedService<ProducerWorker>();

var host = builder.Build();

await host.RunAsync();