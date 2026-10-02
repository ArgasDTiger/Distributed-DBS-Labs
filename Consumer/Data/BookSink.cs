using Consumer.Documents;
using Consumer.Kafka;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.Models;

namespace Consumer.Data;

public sealed class BookSink
{
    private readonly IMongoCollection<BookDocument> _booksCollection;

    public BookSink(IMongoClient mongoClient, IOptions<ConsumerConfiguration> options)
    {
        var db = mongoClient.GetDatabase(options.Value.DatabaseName);
        _booksCollection = db.GetCollection<BookDocument>(options.Value.CollectionName);
    }

    public Task UpsertBook(BookMessage msg, CancellationToken ct)
    {
        var doc = new BookDocument(
            msg.Id,
            msg.Title,
            msg.Author,
            msg.Isbn,
            msg.Genre,
            msg.PublishedYear,
            msg.Price,
            msg.IsAvailable,
            msg.UpdatedAt,
            DateTimeOffset.UtcNow);

        var filter = Builders<BookDocument>.Filter.Eq(d => d.Id, doc.Id);
        var options = new ReplaceOptions
        {
            IsUpsert = true
        };

        return _booksCollection.ReplaceOneAsync(filter, doc, options, ct);
    }
}