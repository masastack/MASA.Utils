namespace MASA.Utils.Development.Dapr;

public class ProcessOptions
{
    public string Id { get; set; } = default!;

    /// <summary>
    /// Process Name
    /// </summary>
    public string Name { get; set; } = default!;

    public int Port { get; set; }

    /// <summary>
    /// Process parameters
    /// </summary>
    public string Arguments { get; set; } = default!;
}
