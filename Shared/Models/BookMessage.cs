namespace Shared.Models;

public sealed record BookMessage(
    Guid Id,
    string Title,
    string Author,
    string Isbn,
    string Genre,
    int PublishedYear,
    decimal Price,
    bool IsAvailable,
    DateTimeOffset UpdatedAt);