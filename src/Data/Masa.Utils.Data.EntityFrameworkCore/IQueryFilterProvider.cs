namespace Masa.Utils.Data.EntityFrameworkCore;

public interface IQueryFilterProvider
{
    LambdaExpression OnExecuting(IMutableEntityType entityType);
}
