namespace Producer.Entities;

public sealed class SyncCursor
{
    public int Id { get; private set; } = 1;
    public DateTimeOffset LastProcessedAt { get; private set; }
    public int ProcessedAtSameTimestamp { get; private set; }

    public void Advance(DateTimeOffset timestamp, int countAtTimestamp)
    {
        if (timestamp > LastProcessedAt)
        {
            LastProcessedAt = timestamp;
            ProcessedAtSameTimestamp = countAtTimestamp;
        }
        else
        {
            ProcessedAtSameTimestamp += countAtTimestamp;
        }
    }
}