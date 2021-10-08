namespace MASA.Utils.Data.EntityFrameworkCore;

public interface ISaveChangesFilter
{
    void OnExecuting(ChangeTracker changeTracker);
}
