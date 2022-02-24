namespace Masa.Utils.Development.Dapr.AspNetCore;

public class DaprBackgroundOptions
{
    private int _maxRetryTimes = 5;

    /// <summary>
    /// default: 5
    /// </summary>
    public int MaxRetryTimes
    {
        get => _maxRetryTimes;
        set
        {
            if (value <= 0)
                throw new ArgumentException($"{nameof(MaxRetryTimes)} Must be greater than 0");

            _maxRetryTimes = value;
        }
    }

    private int _defaultTime = 3000;

    /// <summary>
    /// default: 3000ms
    /// </summary>
    public int Time
    {
        get => _defaultTime;
        set
        {
            if (value <= 0)
                throw new ArgumentException($"{nameof(MaxRetryTimes)} Must be greater than 0");

            _defaultTime = value;
        }
    }
}
