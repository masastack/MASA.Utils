namespace Masa.Utils.Data.EntityFrameworkCore.Internal;

internal class NullDisposable : IDisposable
{
    public static NullDisposable Instance { get; } = new();

    public void Dispose()
    {
    }
}
