namespace Masa.Utils.Development.Dapr.Internal;

internal class Const
{
    public const string DEFAULT_APPID_DELIMITER = "-";

    public const string DEFAULT_FILE_NAME = "dapr";

    public const string DEFAULT_ARGUMENT_PREFIX = "--";

    /// <summary>
    /// Heartbeat detection interval, used to detect dapr status
    /// </summary>
    public const int DEFAULT_HEARTBEATINTERVAL = 5000;
}
