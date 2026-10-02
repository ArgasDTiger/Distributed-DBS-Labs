using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Producer.Configurations;
using Producer.Data;
using Producer.Entities;
using Producer.Extensions;
using Shared.Models;

namespace Producer.Kafka;

public sealed partial class ProducerWorker(
    IDbContextFactory<BooksDbContext> dbFactory,
    BookPublisher publisher,
    IOptions<ProducerConfiguration> options,
    ILogger<ProducerWorker> logger)
    : BackgroundService
{
    private readonly ProducerConfiguration _config = options.Value;

    private DateTimeOffset _lastProcessedAt = DateTimeOffset.MinValue;
    private int _offsetAtCurrentTimestamp;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await using (var db = await dbFactory.CreateDbContextAsync(ct))
        {
            var cursor = await db.Set<SyncCursor>().FindAsync([1], ct);
            _lastProcessedAt = cursor?.LastProcessedAt ?? DateTimeOffset.MinValue;
            _offsetAtCurrentTimestamp = cursor?.ProcessedAtSameTimestamp ?? 0;
        }

        using var timer = new PeriodicTimer(_config.PollingInterval);

        while (await timer.WaitForNextTickAsync(ct))
        {
            await PollAndPublish(ct);
        }
    }

    private async Task PollAndPublish(CancellationToken ct)
    {
        LogPollingCycleStarted(_lastProcessedAt);
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync(ct);

            var books = await db.Set<Book>()
                .AsNoTracking()
                .Where(b => b.UpdatedAt >= _lastProcessedAt)
                .OrderBy(b => b.UpdatedAt)
                .Skip(_offsetAtCurrentTimestamp)
                .Take(_config.BatchSize)
                .ToImmutableArrayAsync(ct);

            if (books.Length == 0)
                return;

            LogChangesFound(books.Length, _lastProcessedAt);

            foreach (var book in books)
            {
                var bookMsg = new BookMessage(
                    book.Id,
                    book.Title,
                    book.Author,
                    book.Isbn,
                    book.Genre,
                    book.PublishedYear,
                    book.Price,
                    book.IsAvailable,
                    book.UpdatedAt);

                await publisher.Publish(bookMsg, ct);

                if (book.UpdatedAt > _lastProcessedAt)
                {
                    _lastProcessedAt = book.UpdatedAt;
                    _offsetAtCurrentTimestamp = 1;
                }
                else
                {
                    _offsetAtCurrentTimestamp++;
                }
            }

            await using var saveDb = await dbFactory.CreateDbContextAsync(ct);
            var cursor = await saveDb.Set<SyncCursor>().FindAsync([1], ct)
                         ?? saveDb.Set<SyncCursor>().Add(new SyncCursor()).Entity;
            cursor.Advance(_lastProcessedAt, _offsetAtCurrentTimestamp);
            await saveDb.SaveChangesAsync(ct);

            LogMessagesPublished(books.Length, _lastProcessedAt);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            LogPollingCycleFailed(ex);
        }
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "Polling cycle started. Current cursor: {Cursor}.")]
    private partial void LogPollingCycleStarted(DateTimeOffset cursor);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information,
        Message = "Found {Count} changed book(s) since {Cursor}. Publishing...")]
    private partial void LogChangesFound(int count, DateTimeOffset cursor);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information,
        Message = "Published {Count} message(s). New cursor: {Cursor}.")]
    private partial void LogMessagesPublished(int count, DateTimeOffset cursor);

    [LoggerMessage(EventId = 4, Level = LogLevel.Error,
        Message = "Error during polling cycle. Will retry on next tick.")]
    private partial void LogPollingCycleFailed(Exception exception);
}