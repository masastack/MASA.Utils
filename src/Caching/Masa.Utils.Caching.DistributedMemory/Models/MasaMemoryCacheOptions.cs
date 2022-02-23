namespace Masa.Utils.Caching.DistributedMemory.Models;

/// <summary>
/// The MASA memory cache options.
/// </summary>
public class MasaMemoryCacheOptions : MemoryCacheOptions
{
    /// <summary>
    /// Gets or sets the <see cref="SubscribeKeyType"/>.
    /// </summary>
    public SubscribeKeyTypes SubscribeKeyType { get; set; } = SubscribeKeyTypes.ValueTypeFullNameAndKey;

    /// <summary>
    /// Gets or sets the prefix of subscribe key.
    /// </summary>
    public string SubscribeKeyPrefix { get; set; } = string.Empty;
}
