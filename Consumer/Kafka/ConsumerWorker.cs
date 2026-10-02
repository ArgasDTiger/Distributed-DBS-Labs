using System.Text.Json;
using Confluent.Kafka;
using Consumer.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Models;

namespace Consumer.Kafka;

public sealed partial class ConsumerWorker(
    BookSink sink,
    string bootstrapServers,
    IOptions<ConsumerConfiguration> options,
    ILogger<ConsumerWorker> logger)
    : BackgroundService
{
    private readonly ConsumerConfiguration _config = options.Value;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected override Task ExecuteAsync(CancellationToken ct)
    {
        return ConsumeLoop(ct);
    }

    private async Task ConsumeLoop(CancellationToken ct)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = _config.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoOffsetStore = false,
            EnableAutoCommit = true
        };

        using var consumer = new ConsumerBuilder<string, string>(config)
            .SetErrorHandler((_, err) => LogKafkaConsumerError(err.Reason, err.IsFatal))
            .Build();

        consumer.Subscribe(_config.TopicName);

        while (!ct.IsCancellationRequested)
        {
            ConsumeResult<string, string>? result = null;

            try
            {
                result = consumer.Consume(_config.ConsumeTimeout);

                if (result is null || result.IsPartitionEOF)
                {
                    continue;
                }

                var message = JsonSerializer.Deserialize<BookMessage>(result.Message.Value, SerializerOptions);

                if (message is null)
                {
                    LogMessageDeserializationFailed(result.Topic, result.Partition.Value, result.Offset.Value);
                    consumer.StoreOffset(result);

                    continue;
                }

                await sink.UpsertBook(message, ct);

                consumer.StoreOffset(result);
            }
            catch (ConsumeException ex)
            {
                LogKafkaConsumeException(ex);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                LogMessageProcessingFailed(ex);
                if (result is not null)
                {
                    consumer.StoreOffset(result);
                }
            }
        }

        consumer.Close();
    }
    
    [LoggerMessage(EventId = 1, Level = LogLevel.Warning, Message = "Failed to deserialize message at {Topic}[{Partition}]@{Offset}. Skipping.")]
    private partial void LogMessageDeserializationFailed(string topic, int partition, long offset);
    
    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "Kafka consume exception.")]
    private partial void LogKafkaConsumeException(Exception exception);

    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "Unhandled error processing message. Will skip to next message.")]
    private partial void LogMessageProcessingFailed(Exception exception);

    [LoggerMessage(EventId = 4, Level = LogLevel.Error, Message = "Kafka consumer error: {Reason} (fatal={IsFatal}).")]
    private partial void LogKafkaConsumerError(string reason, bool isFatal);
}