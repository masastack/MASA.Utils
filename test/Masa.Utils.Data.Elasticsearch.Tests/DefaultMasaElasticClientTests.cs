namespace Masa.Utils.Data.Elasticsearch.Tests;

[TestClass]
public class DefaultMasaElasticClientTests
{
    private MasaElasticsearchBuilder _builder = default!;

    [TestInitialize]
    public void Initialize()
    {
        IServiceCollection service = new ServiceCollection();
        _builder = service.AddElasticsearchClient("es", "http://localhost:9200");
    }

    [TestMethod]
    public async Task CreateDocumentAsync()
    {
        string indexName = "user_index";
        var countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName));
        Assert.IsTrue(!countResponse.IsValid);

        var createResponse = await _builder.Client.CreateDocumentAsync(new CreateDocumentRequest<object>(indexName, new
        {
            id = Guid.NewGuid()
        }, Guid.NewGuid().ToString()));
        Assert.IsTrue(createResponse.IsValid);

        Thread.Sleep(1000);
        countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 1);
        await _builder.Client.DeleteIndexAsync(indexName);
    }

    [TestMethod]
    public async Task BindAliasAsync()
    {
        string indexName = "user_index_1";
        string indexName2 = "user_index_2";
        string alias = "userIndex";

        await _builder.Client.CreateIndexAsync(indexName);
        await _builder.Client.CreateIndexAsync(indexName2);

        await _builder.Client.CreateDocumentAsync(new CreateDocumentRequest<object>(indexName, new
        {
            id = Guid.NewGuid()
        }, Guid.NewGuid().ToString()));
        await _builder.Client.CreateDocumentAsync(new CreateDocumentRequest<object>(indexName2, new
        {
            id = Guid.NewGuid()
        }, Guid.NewGuid().ToString()));

        Thread.Sleep(1000);
        var countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 1);
        countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName2));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 1);

        await _builder.Client.BindAliasAsync(new BindAliasIndexOptions(alias, new[] { indexName, indexName2 }));
        countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(alias));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 2);

        await _builder.Client.DeleteIndexAsync(indexName);
        await _builder.Client.DeleteIndexAsync(indexName2);
    }

    [TestMethod]
    public async Task DeleteIndexByAliasAsync()
    {
        string indexName = "user_index_1";
        string indexName2 = "user_index_2";
        string alias = "userIndex";

        await _builder.Client.CreateIndexAsync(indexName);
        await _builder.Client.CreateIndexAsync(indexName2);

        await _builder.Client.CreateDocumentAsync(new CreateDocumentRequest<object>(indexName, new
        {
            id = Guid.NewGuid()
        }, Guid.NewGuid().ToString()));
        await _builder.Client.CreateDocumentAsync(new CreateDocumentRequest<object>(indexName2, new
        {
            id = Guid.NewGuid()
        }, Guid.NewGuid().ToString()));

        Thread.Sleep(1000);
        var countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 1);
        countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName2));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 1);

        await _builder.Client.BindAliasAsync(new BindAliasIndexOptions(alias, new[] { indexName, indexName2 }));
        countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(alias));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 2);

        await _builder.Client.DeleteIndexByAliasAsync(alias);
        countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(alias));
        Assert.IsTrue(!countResponse.IsValid);
    }

    [TestMethod]
    public async Task ClearDocumentAsync()
    {
        string indexName = "user_index_1";
        string indexName2 = "user_index_2";
        string alias = "userIndex";

        await _builder.Client.CreateIndexAsync(indexName);
        await _builder.Client.CreateIndexAsync(indexName2);

        await _builder.Client.CreateDocumentAsync(new CreateDocumentRequest<object>(indexName, new
        {
            id = Guid.NewGuid()
        }, Guid.NewGuid().ToString()));
        await _builder.Client.CreateDocumentAsync(new CreateDocumentRequest<object>(indexName2, new
        {
            id = Guid.NewGuid()
        }, Guid.NewGuid().ToString()));

        Thread.Sleep(1000);
        var countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 1);
        countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName2));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 1);

        await _builder.Client.BindAliasAsync(new BindAliasIndexOptions(alias, new[] { indexName, indexName2 }));
        countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(alias));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 2);

        await _builder.Client.ClearDocumentAsync(alias);
        countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(alias));
        Assert.IsTrue(countResponse.IsValid);

        await _builder.Client.DeleteIndexByAliasAsync(alias);
    }
}
