namespace Masa.Utils.Caller.Core;

public class CallerOptions
{
    internal readonly List<CallerRelations> Callers = new();

    public IServiceCollection Services { get; }

    private Assembly[] _assemblies = AppDomain.CurrentDomain.GetAssemblies();

    public Assembly[] Assemblies
    {
        get => _assemblies;
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(Assemblies));

            _assemblies = value;
        }
    }

    public ServiceLifetime CallerLifetime { get; set; }

    public JsonSerializerOptions? JsonSerializerOptions { get; set; }

    public CallerOptions(IServiceCollection services)
    {
        Services = services;
        CallerLifetime = ServiceLifetime.Scoped;
    }

    public void AddCaller(string name, bool isDefault, Func<IServiceProvider, ICallerProvider> func)
    {
        if (Callers.Any(c => c.Name == name))
            throw new ArgumentException("The current name already exists, please change the name");

        Callers.Add(new CallerRelations(name, isDefault, func));
    }
}
