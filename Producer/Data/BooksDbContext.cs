using Microsoft.EntityFrameworkCore;

namespace Producer.Data;

public sealed class BooksDbContext(DbContextOptions<BooksDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BooksDbContext).Assembly);
    }
}