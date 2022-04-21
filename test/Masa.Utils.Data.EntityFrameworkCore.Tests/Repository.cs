namespace Masa.Utils.Data.EntityFrameworkCore.Test;

internal class Repository
{
    private readonly TestDbContext _testDbContext;

    public Repository(TestDbContext testDbContext) => _testDbContext = testDbContext;

    public Task<List<Student>> GetPaginatedListAsync(int skip, int take, CancellationToken cancellationToken = default)
        => _testDbContext.Set<Student>().Skip(skip).Take(take).ToListAsync(cancellationToken);

    public virtual async Task<PaginatedList<Student>> GetPaginatedListAsync(PaginatedOptions options, CancellationToken cancellationToken = default)
    {
        var result = await GetPaginatedListAsync(
            (options.Page - 1) * options.PageSize,
            options.PageSize <= 0 ? int.MaxValue : options.PageSize,
            cancellationToken
        );

        var total = await GetCountAsync(cancellationToken);

        return new PaginatedList<Student>()
        {
            Total = total,
            Result = result,
            TotalPages = (int)Math.Ceiling(total / (decimal)options.PageSize)
        };
    }

    public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        => await _testDbContext.Set<Student>().LongCountAsync(cancellationToken);
}