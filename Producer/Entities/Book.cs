namespace Producer.Entities;

public sealed class Book
{
    public Guid Id { get; init; }
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Isbn { get; set; } = null!;
    public string Genre { get; set; }  = null!;
    public int PublishedYear { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}