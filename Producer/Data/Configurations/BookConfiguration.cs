using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Producer.Entities;

namespace Producer.Data.Configurations;

public sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable(nameof(Book));

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(b => b.Title)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(b => b.Author)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(b => b.Isbn)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(b => b.Genre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.PublishedYear)
            .IsRequired();

        builder.Property(b => b.Price)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(b => b.IsAvailable)
            .IsRequired();

        builder.Property(b => b.UpdatedAt)
            .IsRequired();

        builder.HasIndex(b => b.UpdatedAt);

        builder.HasIndex(b => b.Isbn)
            .IsUnique();
    }
}