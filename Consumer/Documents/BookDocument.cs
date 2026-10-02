using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Consumer.Documents;

[BsonIgnoreExtraElements]
public sealed record BookDocument(
    [property: BsonId]
    [property: BsonRepresentation(BsonType.String)]
    Guid Id,
    [property: BsonElement("title")]
    string Title,
    [property: BsonElement("author")]
    string Author,
    [property: BsonElement("isbn")]
    string Isbn,
    [property: BsonElement("genre")]
    string Genre,
    [property: BsonElement("publishedYear")]
    int PublishedYear,
    [property: BsonElement("price")]
    [property: BsonRepresentation(BsonType.Decimal128)]
    decimal Price,
    [property: BsonElement("isAvailable")]
    bool IsAvailable,
    [property: BsonElement("updatedAt")]
    DateTimeOffset UpdatedAt,
    [property: BsonElement("syncedAt")]
    DateTimeOffset SyncedAt);