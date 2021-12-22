namespace MASA.Utils.Data.Elasticsearch;

public class DefaultMasaElasticClient : IMasaElasticClient
{
    private readonly IElasticClient _elasticClient;

    public bool ExistDefaultIndex
        => !string.IsNullOrWhiteSpace(_elasticClient.ConnectionSettings.DefaultIndex);

    public string DefaultIndex
        => ExistDefaultIndex
            ? _elasticClient.ConnectionSettings.DefaultIndex
            : throw new ArgumentNullException("The default IndexName is not set", nameof(DefaultIndex));

    public DefaultMasaElasticClient(IElasticClient elasticClient)
        => _elasticClient = elasticClient;

    #region index manage

    public Task<Response.ExistsResponse> IndexExistAsync(
        CancellationToken cancellationToken = default)
        => IndexExistAsync(DefaultIndex, cancellationToken);

    public async Task<Response.ExistsResponse> IndexExistAsync(
        string indexName,
        CancellationToken cancellationToken = default)
    {
        IIndexExistsRequest request = new IndexExistsDescriptor(indexName);
        return new Response.ExistsResponse(await _elasticClient.Indices.ExistsAsync(request, cancellationToken));
    }

    public Task<Response.CreateIndexResponse> CreateIndexAsync(
        CreateIndexOptions? options = null,
        CancellationToken cancellationToken = default)
        => CreateIndexAsync(DefaultIndex, options, cancellationToken);

    public async Task<Response.CreateIndexResponse> CreateIndexAsync(
        string indexName,
        CreateIndexOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(indexName))
            throw new ArgumentException(nameof(indexName));

        ICreateIndexRequest request = new CreateIndexDescriptor(indexName);
        if (options != null)
        {
            request.Settings = options.IndexSettings;
            request.Aliases = options.Aliases;
            request.Mappings = options.Mappings;
        }

        return new Response.CreateIndexResponse(await _elasticClient.Indices.CreateAsync(request, cancellationToken));
    }

    public Task<Response.DeleteIndexResponse> DeleteIndexAsync(
        DeleteIndexOptions? options = null,
        CancellationToken cancellationToken = default)
        => DeleteIndexAsync(DefaultIndex, options, cancellationToken);

    public async Task<Response.DeleteIndexResponse> DeleteIndexAsync(
        string indexName,
        DeleteIndexOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (options != null && !string.IsNullOrEmpty(options.Alias))
            return await DeleteAliasAsync(indexName, options.Alias, cancellationToken);

        return await DeleteIndexAsync(indexName, cancellationToken);
    }

    private async Task<Response.DeleteIndexResponse> DeleteAliasAsync(
        string indexName,
        string alias,
        CancellationToken cancellationToken = default)
    {
        IDeleteAliasRequest request = new DeleteAliasDescriptor(indexName, alias);
        return new Response.DeleteIndexResponse(await _elasticClient.Indices.DeleteAliasAsync(request, cancellationToken));
    }

    private async Task<Response.DeleteIndexResponse> DeleteIndexAsync(
        string indexName,
        CancellationToken cancellationToken = default)
    {
        IDeleteIndexRequest request = new DeleteIndexDescriptor(indexName);
        return new Response.DeleteIndexResponse(await _elasticClient.Indices.DeleteAsync(request, cancellationToken));
    }

    #endregion

    #region document manage

    public Task<Response.CreateResponse> CreateDocumentAsync<TDocument>(TDocument document,
        string? documentId = null,
        CancellationToken cancellationToken = default) where TDocument : class
        => CreateDocumentAsync(DefaultIndex, document, documentId, cancellationToken);

    public async Task<Response.CreateResponse> CreateDocumentAsync<TDocument>(
        string indexName,
        TDocument document,
        string? documentId = null,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        ICreateRequest<TDocument> request = documentId != null
            ? new CreateDescriptor<TDocument>(indexName, new Id(documentId))
            : new CreateDescriptor<TDocument>(indexName);
        request.Document = document;
        return new Response.CreateResponse(await _elasticClient.CreateDocumentAsync(request, cancellationToken));
    }

    public Task<Response.DeleteResponse> DeleteDocumentAsync(
        string documentId,
        CancellationToken cancellationToken = default)
        => DeleteDocumentAsync(DefaultIndex, documentId, cancellationToken);

    public async Task<Response.DeleteResponse> DeleteDocumentAsync(
        string indexName,
        string documentId,
        CancellationToken cancellationToken = default)
    {
        IDeleteRequest request = new DeleteRequest(indexName, new Id(documentId));
        return new Response.DeleteResponse(await _elasticClient.DeleteAsync(request, cancellationToken));
    }

    public async Task<Response.SearchResponse<TDocument>> GetListAsync<TDocument>(
        QueryOptions options,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var ret = await QueryString<TDocument>(
            options.IndexName,
            options.Skip,
            options.Take,
            options.Fields,
            options.Query,
            options.Operator,
            cancellationToken);
        return new Response.SearchResponse<TDocument>(ret);
    }

    public async Task<SearchPaginatedResponse<TDocument>> GetPaginatedListAsync<TDocument>(
        PaginatedOptions options,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var ret = await QueryString<TDocument>(
            options.IndexName,
            (options.Page - 1) * options.PageSize,
            options.PageSize,
            options.Fields,
            options.Query,
            options.Operator,
            cancellationToken);
        return new SearchPaginatedResponse<TDocument>(options.PageSize, ret);
    }

    private Task<ISearchResponse<TDocument>> QueryString<TDocument>(
        string? indexName,
        int skip,
        int take,
        string[] fields,
        string query,
        Operator @operator,
        CancellationToken cancellationToken = default)
        where TDocument : class
    {
        return _elasticClient.SearchAsync<TDocument>(s => s
            .Index(indexName ?? DefaultIndex)
            .From(skip)
            .Size(take)
            .Query(q => q
                .QueryString(qs => qs
                    .Fields(fields)
                    .Query(query)
                    .DefaultOperator(@operator)
                )
            ), cancellationToken);
    }

    #endregion
}
