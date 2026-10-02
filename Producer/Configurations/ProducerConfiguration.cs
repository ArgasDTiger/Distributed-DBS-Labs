using System.ComponentModel.DataAnnotations;

namespace Producer.Configurations;

public sealed class ProducerConfiguration
{
    [Required] public required string TopicName { get; init; }

    public TimeSpan PollingInterval { get; init; } = TimeSpan.FromSeconds(10);

    [Range(1, 10_000)] public int BatchSize { get; init; } = 500;

    public TimeSpan DeliveryTimeout { get; init; } = TimeSpan.FromSeconds(30);
}