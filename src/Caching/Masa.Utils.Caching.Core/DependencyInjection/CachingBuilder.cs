namespace Masa.Utils.Caching.Core.DependencyInjection;

public class CachingBuilder : ICachingBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CachingBuilder"/> class.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="name"></param>
    public CachingBuilder(IServiceCollection services, string name)
    {
        Services = services;
        Name = name;
    }

    /// <inheritdoc />
    public IServiceCollection Services { get; private set; }

    /// <inheritdoc />
    public string Name { get; private set; }
}
