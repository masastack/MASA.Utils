namespace Masa.Utils.Data.EntityFrameworkCore.Internal;

internal class DataFilterState
{
    public bool Enabled { get; set; }

    public DataFilterState(bool enabled)
    {
        Enabled = enabled;
    }
}
