namespace Masa.Utils.Data.EntityFrameworkCore.Filters;

public interface ISaveChangesFilter
{
    void OnExecuting(ChangeTracker changeTracker);
}
