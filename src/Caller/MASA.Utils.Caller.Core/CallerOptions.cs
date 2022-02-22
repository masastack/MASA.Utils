namespace MASA.Utils.Caller.Core;

public class CallerOptions
{
    internal List<CallerRelations> Callers = new();

    public IServiceCollection Services { get; }

    public JsonSerializerOptions? JsonSerializerOptions { get; set; }

    public CallerOptions(IServiceCollection services, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Services = services;
        JsonSerializerOptions = jsonSerializerOptions;
    }

    public void AddCaller(string name, bool isDefault, Func<IServiceProvider, ICallerProvider> func)
    {
        if (Callers.Any(c => c.Name == name))
            throw new ArgumentException("The current name already exists, please change the name");

        Callers.Add(new CallerRelations(name, isDefault, func));
    }
}
