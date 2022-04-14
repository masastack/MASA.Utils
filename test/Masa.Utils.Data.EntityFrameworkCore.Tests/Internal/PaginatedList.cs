namespace Masa.Utils.Data.EntityFrameworkCore.Test.Internal;

internal class PaginatedList<TEntity>
    where TEntity : class
{
    public long Total { get; set; }

    public int TotalPages { get; set; }

    public List<TEntity> Result { get; set; } = default!;
}
