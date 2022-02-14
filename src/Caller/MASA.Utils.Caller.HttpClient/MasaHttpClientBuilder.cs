namespace MASA.Utils.Caller.HttpClient;

public class MasaHttpClientBuilder
{
    private string _name = default!;

    public string Name
    {
        get => _name;
        set
        {
            if (value is null)
                throw new ArgumentNullException(Name);

            _name = value;
        }
    }

    public string BaseAPI { get; set; }

    public bool IsDefault { get; set; } = false;

    public Action<System.Net.Http.HttpClient>? Configure { get; set; }

    public MasaHttpClientBuilder() : this("http", null)
    {
    }

    public MasaHttpClientBuilder(string name, Action<System.Net.Http.HttpClient>? configure)
        : this(name, string.Empty, configure)
    {
    }

    public MasaHttpClientBuilder(string name, string baseAPI, Action<System.Net.Http.HttpClient>? configure)
    {
        Name = name;
        BaseAPI = baseAPI;
        Configure = configure;
    }
}

