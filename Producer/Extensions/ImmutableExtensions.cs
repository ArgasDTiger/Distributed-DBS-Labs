using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;

namespace Producer.Extensions;

public static class ImmutableExtensions
{
    public static async Task<ImmutableArray<TSource>> ToImmutableArrayAsync<TSource>(
        this IQueryable<TSource> source, CancellationToken cancellationToken)
    {
        var builder = ImmutableArray.CreateBuilder<TSource>();
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            builder.Add(element);
        }

        return builder.ToImmutable();
    }
}