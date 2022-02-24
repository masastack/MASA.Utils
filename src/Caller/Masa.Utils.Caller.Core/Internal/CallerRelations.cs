namespace Masa.Utils.Caller.Core.Internal;

internal class CallerRelations
{
    public string Name { get; } = default!;

    public bool IsDefault { get; }

    public Func<IServiceProvider, ICallerProvider> Func { get; } = default!;

    public CallerRelations(string name, bool isDefault, Func<IServiceProvider, ICallerProvider> func)
    {
        Name = name;
        IsDefault = isDefault;
        Func = func;
    }
}
