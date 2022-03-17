namespace Masa.Utils.Data.EntityFrameworkCore.Internal;

internal class DisposeAction : IDisposable
{
    private readonly Action _action;

    public DisposeAction(Action action) => _action = action;

    public void Dispose() => _action.Invoke();
}
