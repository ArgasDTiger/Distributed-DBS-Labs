using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Producer.Configurations;
using Shared.Models;

namespace Producer.Kafka;

public sealed class BookPublisher : IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = false
    };

    public BookPublisher(string bootstrapServers, IOptions<ProducerConfiguration> options)
    {
        _topic = options.Value.TopicName;

        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            EnableIdempotence = true,
            Acks = Acks.All,
            MessageTimeoutMs = (int)options.Value.DeliveryTimeout.TotalMilliseconds
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task Publish(BookMessage message, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(message, SerializerOptions);

        var kafkaMessage = new Message<string, string>
        {
            Key = message.Id.ToString(),
            Value = json
        };

        await _producer.ProduceAsync(_topic, kafkaMessage, ct);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}