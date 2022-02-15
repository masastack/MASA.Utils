namespace MASA.Utils.Development.Dapr.Configurations;

public class DaprRuntimeOptions
{
    [JsonPropertyName("appId")]
    public string AppId { get; set; }

    [JsonPropertyName("httpPort")]
    public int HttpPort { get; set; }

    [JsonPropertyName("grpcPort")]
    public int GrpcPort { get; set; }

    [JsonPropertyName("appPort")]
    public int AppPort { get; set; }

    [JsonPropertyName("metricsEnabled")]
    public bool MetricsEnabled { get; set; }

    [JsonPropertyName("command")]
    public string Command { get; set; }

    [JsonPropertyName("pid")]
    public int PId { get; set; }
}
