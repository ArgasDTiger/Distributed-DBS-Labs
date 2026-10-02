using System.ComponentModel.DataAnnotations;

namespace Consumer.Kafka;

public sealed record ConsumerConfiguration
{
    [Required]
    public required string TopicName { get; init; }

    [Required]
    public required string GroupId { get; init; }

    [Required]
    public required string DatabaseName { get; init; }

    [Required]
    public required string CollectionName { get; init; }

    public required TimeSpan ConsumeTimeout { get; init; }
}