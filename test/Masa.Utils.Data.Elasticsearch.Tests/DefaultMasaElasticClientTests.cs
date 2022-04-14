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
    public async Task TestCreateDocumentAsyncReturnCountIs1()
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
    public async Task TestCreateMultiDocumentAsyncReturnCountIs2()
    {
        string indexName = "user_index";
        var countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName));
        Assert.IsTrue(!countResponse.IsValid);

        string id = Guid.NewGuid().ToString();
        string id2 = Guid.NewGuid().ToString();
        var createMultiResponse = await _builder.Client.CreateMultiDocumentAsync(new CreateMultiDocumentRequest<object>(indexName)
        {
            Items = new List<SingleDocumentBaseRequest<object>>()
            {
                new(new
                {
                    Id = Guid.NewGuid()
                }, id),
                new(new
                {
                    Id = Guid.NewGuid()
                }, id2)
            }
        });
        Assert.IsTrue(createMultiResponse.IsValid &&
            createMultiResponse.Items.Count == 2 &&
            createMultiResponse.Items.Count(r => r.IsValid) == 2);

        Thread.Sleep(1000);
        countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 2);
        await _builder.Client.DeleteIndexAsync(indexName);
    }

    [TestMethod]
    public async Task TestDeleteDocumentAsyncReturnCountIs0()
    {
        string indexName = "user_index";
        var id = Guid.NewGuid();
        var createResponse = await _builder.Client.CreateDocumentAsync(new CreateDocumentRequest<object>(indexName, new
        {
            id = id
        }, id.ToString()));
        Assert.IsTrue(createResponse.IsValid);

        var deleteResponse = await _builder.Client.DeleteDocumentAsync(new DeleteDocumentRequest(indexName, id.ToString()));
        Assert.IsTrue(deleteResponse.IsValid);

        Thread.Sleep(1000);
        var countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 0);

        await _builder.Client.DeleteIndexAsync(indexName);
    }

    [TestMethod]
    public async Task TestDeleteMultiDocumentAsyncReturnCountIs1()
    {
        string indexName = "user_index";
        string id = Guid.NewGuid().ToString();
        string id2 = Guid.NewGuid().ToString();
        await _builder.Client.CreateMultiDocumentAsync(new CreateMultiDocumentRequest<object>(indexName)
        {
            Items = new List<SingleDocumentBaseRequest<object>>()
            {
                new(new
                {
                    Id = Guid.NewGuid()
                }, id),
                new(new
                {
                    Id = Guid.NewGuid()
                }, id2)
            }
        });

        var deleteResponse = await _builder.Client.DeleteMultiDocumentAsync(new DeleteMultiDocumentRequest(indexName, id, id2));
        Assert.IsTrue(deleteResponse.IsValid && deleteResponse.Data.Count == 2 && deleteResponse.Data.Count(r => r.IsValid) == 2);

        Thread.Sleep(1000);
        var countResponse = await _builder.Client.DocumentCountAsync(new CountDocumentRequest(indexName));
        Assert.IsTrue(countResponse.IsValid && countResponse.Count == 0);

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
