namespace Masa.Utils.Data.EntityFrameworkCore.Filters;

public class SoftDeleteSaveChangesFilter : ISaveChangesFilter
{
    private readonly MasaDbContextOptions _masaDbContextOptions;

    public SoftDeleteSaveChangesFilter(MasaDbContextOptions masaDbContextOptions) => _masaDbContextOptions = masaDbContextOptions;

    public void OnExecuting(ChangeTracker changeTracker)
    {
        if (!_masaDbContextOptions.EnableSoftDelete)
            return;

        changeTracker.DetectChanges();
        foreach (var entity in changeTracker.Entries().Where(entry => entry.State == EntityState.Deleted))
        {
            entity.State = EntityState.Modified;
            entity.CurrentValues[nameof(ISoftDelete.IsDeleted)] = true;
        }
    }
}
