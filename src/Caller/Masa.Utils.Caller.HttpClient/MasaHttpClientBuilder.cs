namespace Masa.Utils.Caller.HttpClient;

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

    public string BaseApi { get; set; }

    public bool IsDefault { get; set; } = false;

    public Action<System.Net.Http.HttpClient>? Configure { get; set; }

    public MasaHttpClientBuilder() : this("http", null)
    {
    }

    public MasaHttpClientBuilder(string name, Action<System.Net.Http.HttpClient>? configure)
        : this(name, string.Empty, configure)
    {
    }

    public MasaHttpClientBuilder(string name, string baseApi, Action<System.Net.Http.HttpClient>? configure)
    {
        Name = name;
        BaseApi = baseApi;
        Configure = configure;
    }
}

