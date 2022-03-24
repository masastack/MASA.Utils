namespace Masa.Utils.Ldap.Extensions;

public static class AsyncEnumerableExtensions
{
    /// <summary>
    /// Asynchronously materializes the subject <see cref="IAsyncEnumerable{T}"/> into a list.
    /// </summary>
    public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> enumerable, CancellationToken cancellationToken = default)
    {
        var list = new List<T>();
        await foreach (var element in enumerable.WithCancellation(cancellationToken))
        {
            list.Add(element);
        }

        return list;
    }
}
