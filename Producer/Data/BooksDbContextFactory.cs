using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Producer.Data;

public sealed class BooksDbContextFactory : IDesignTimeDbContextFactory<BooksDbContext>
{
    public BooksDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<BooksDbContext>()
            .UseNpgsql(
                "Host=localhost;Port=5432;Database=booksdb;Username=postgres;Password=postgres")
            .Options;
 
        return new BooksDbContext(options);
    }
}
