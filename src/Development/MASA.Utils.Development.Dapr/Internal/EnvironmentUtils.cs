namespace MASA.Utils.Development.Dapr.Internal;

internal class EnvironmentUtils
{
    public static void TryAdd(string environment, Func<string?> func)
    {
        var value = Environment.GetEnvironmentVariable(environment);
        if (value == null)
        {
            Environment.SetEnvironmentVariable(environment, func.Invoke());
        }
    }
}
