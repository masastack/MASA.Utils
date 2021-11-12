namespace MASA.Utils.Caller.HttpClient;

public class MasaHttpClientBuilder
{
    private string _name = default!;

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            if (value is null)
                throw new ArgumentNullException(Name);

            _name = value;
        }
    }

    public string BaseAPI { get; set; } = string.Empty;

    public bool IsDefault { get; set; } = false;

    public Action<System.Net.Http.HttpClient>? Configure { get; set; }

    public JsonSerializerOptions? JsonSerializerOptions { get; set; }

    public MasaHttpClientBuilder() : this("http", null)
    {
    }

    public MasaHttpClientBuilder(string name, Action<System.Net.Http.HttpClient>? configure)
        : this(name, configure, null)
    {
    }

    public MasaHttpClientBuilder(string name, Action<System.Net.Http.HttpClient>? configure, JsonSerializerOptions? jsonSerializerOptions = null)
        : this(name, string.Empty, configure, jsonSerializerOptions)
    {
    }

    public MasaHttpClientBuilder(string name, string baseAPI, Action<System.Net.Http.HttpClient>? configure, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Name = name;
        BaseAPI = baseAPI;
        Configure = configure;
        JsonSerializerOptions = jsonSerializerOptions;
    }
}

