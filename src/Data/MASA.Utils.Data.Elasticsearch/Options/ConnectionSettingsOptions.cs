namespace MASA.Utils.Data.Elasticsearch.Options;

public class ConnectionSettingsOptions
{
    internal IConnection? Connection { get; set; }

    internal ConnectionSettings.SourceSerializerFactory? SourceSerializerFactory { get; set; }

    internal IPropertyMappingProvider? PropertyMappingProvider { get; set; }

    public ConnectionSettingsOptions()
    {
        this.Connection = null;
        this.SourceSerializerFactory = null;
        this.PropertyMappingProvider = null;
    }

    public ConnectionSettingsOptions UseConnection(IConnection? connection)
    {
        this.Connection = connection;
        return this;
    }

    public ConnectionSettingsOptions UseSourceSerializerFactory(ConnectionSettings.SourceSerializerFactory? sourceSerializerFactory)
    {
        this.SourceSerializerFactory = sourceSerializerFactory;
        return this;
    }

    public ConnectionSettingsOptions UsePropertyMappingProvider(IPropertyMappingProvider? propertyMappingProvider)
    {
        this.PropertyMappingProvider = propertyMappingProvider;
        return this;
    }
}
