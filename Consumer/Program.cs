using Consumer.Data;
using Consumer.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddOptions<ConsumerConfiguration>()
    .BindConfiguration(nameof(ConsumerConfiguration))
    .ValidateDataAnnotations()
    .ValidateOnStart();

const string mongoConnectionName = "mongo-booksdb";
const string kafkaConnectionName = "kafka";
 
var mongoConnectionString = builder.Configuration.GetConnectionString(mongoConnectionName)
                            ?? throw new InvalidOperationException($"'{mongoConnectionName}' connection string is missing.");
 
var kafkaBootstrapServers = builder.Configuration.GetConnectionString(kafkaConnectionName)
                            ?? throw new InvalidOperationException($"'{kafkaConnectionName}' connection string is missing.");
 
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));
 
builder.Services.AddSingleton<BookSink>();

builder.Services.AddHostedService(sp =>
    new ConsumerWorker(
        sp.GetRequiredService<BookSink>(),
        kafkaBootstrapServers,
        sp.GetRequiredService<IOptions<ConsumerConfiguration>>(),
        sp.GetRequiredService<ILogger<ConsumerWorker>>()));


await builder.Build().RunAsync();