namespace Masa.Utils.Data.EntityFrameworkCore.Internal;

internal class DataFilterState
{
    public bool IsEnabled { get; set; }

    public DataFilterState(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }
}
