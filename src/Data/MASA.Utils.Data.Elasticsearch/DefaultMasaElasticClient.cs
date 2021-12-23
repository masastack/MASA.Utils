namespace MASA.Utils.Data.Elasticsearch;

public class DefaultMasaElasticClient : IMasaElasticClient
{
    private readonly IElasticClient _elasticClient;

    public bool ExistDefaultIndex
        => !string.IsNullOrWhiteSpace(_elasticClient.ConnectionSettings.DefaultIndex);

    public string DefaultIndex
        => ExistDefaultIndex
            ? _elasticClient.ConnectionSettings.DefaultIndex
            : throw new ArgumentNullException(nameof(DefaultIndex), "The default IndexName is not set");

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
        IIndexExistsRequest request = new IndexExistsRequest(indexName);
        return new Response.ExistsResponse(await _elasticClient.Indices.ExistsAsync(request, cancellationToken));
    }

    public Task<Response.Index.CreateIndexResponse> CreateIndexAsync(
        CreateIndexOptions? options = null,
        CancellationToken cancellationToken = default)
        => CreateIndexAsync(DefaultIndex, options, cancellationToken);

    public async Task<Response.Index.CreateIndexResponse> CreateIndexAsync(
        string indexName,
        CreateIndexOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(indexName))
            throw new ArgumentException(nameof(indexName));

        ICreateIndexRequest request = new CreateIndexRequest(indexName);
        if (options != null)
        {
            request.Settings = options.IndexSettings;
            request.Aliases = options.Aliases;
            request.Mappings = options.Mappings;
        }

        return new Response.Index.CreateIndexResponse(await _elasticClient.Indices.CreateAsync(request, cancellationToken));
    }

    public Task<Response.Index.DeleteIndexResponse> DeleteIndexAsync(
        DeleteIndexOptions? options = null,
        CancellationToken cancellationToken = default)
        => DeleteIndexAsync(DefaultIndex, options, cancellationToken);

    public async Task<Response.Index.DeleteIndexResponse> DeleteIndexAsync(
        string indexName,
        DeleteIndexOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (options != null && !string.IsNullOrEmpty(options.Alias))
            return await DeleteAliasAsync(indexName, options.Alias, cancellationToken);

        return await DeleteIndexAsync(indexName, cancellationToken);
    }

    private async Task<Response.Index.DeleteIndexResponse> DeleteAliasAsync(
        string indexName,
        string alias,
        CancellationToken cancellationToken = default)
    {
        IDeleteAliasRequest request = new DeleteAliasRequest(indexName, alias);
        return new Response.Index.DeleteIndexResponse(await _elasticClient.Indices.DeleteAliasAsync(request, cancellationToken));
    }

    private async Task<Response.Index.DeleteIndexResponse> DeleteIndexAsync(
        string indexName,
        CancellationToken cancellationToken = default)
    {
        IDeleteIndexRequest request = new DeleteIndexRequest(indexName);
        return new Response.Index.DeleteIndexResponse(await _elasticClient.Indices.DeleteAsync(request, cancellationToken));
    }

    #endregion

    #region document manage

    public async Task<Response.ExistsResponse> DocumentExistsAsync(
        ExistDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        var newRequest = new DocumentExistsRequest(GetIndexName(request.IndexName), request.DocumentId);
        return new Response.ExistsResponse(await _elasticClient.DocumentExistsAsync(newRequest, cancellationToken));
    }

    /// <summary>
    /// Add a new document
    /// only when the document does not exist
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDocument"></typeparam>
    /// <returns></returns>
    public async Task<Response.CreateResponse> CreateDocumentAsync<TDocument>(
        CreateDocumentRequest<TDocument> request,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        ICreateRequest<TDocument> newRequest = request.Request.DocumentId != null
            ? new CreateRequest<TDocument>(GetIndexName(request.IndexName), new Id(request.Request.DocumentId))
            : new CreateRequest<TDocument>(request.IndexName);
        newRequest.Document = request.Request.Document;
        return new Response.CreateResponse(await _elasticClient.CreateAsync(newRequest, cancellationToken));
    }

    /// <summary>
    /// Add new documents in batches
    /// only when the documents do not exist
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDocument"></typeparam>
    /// <returns></returns>
    public async Task<Response.CreateMultiResponse> CreateMultiDocumentAsync<TDocument>(
        CreateMultiDocumentRequest<TDocument> request,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var indexName = GetIndexName(request.IndexName);
        BulkDescriptor descriptor = new BulkDescriptor(indexName);
        foreach (var item in request.Items)
        {
            descriptor
                .Create<TDocument>(opt => opt.Document(item.Document)
                    .Index(indexName)
                    .Id(item.DocumentId));
        }

        var ret = await _elasticClient.BulkAsync(descriptor, cancellationToken);
        return new Response.CreateMultiResponse(ret);
    }

    /// <summary>
    /// Update or insert document
    /// Overwrite if it exists, add new if it does not exist
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDocument"></typeparam>
    /// <returns></returns>
    public async Task<SetResponse> SetDocumentAsync<TDocument>(
        SetDocumentRequest<TDocument> request,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var indexName = GetIndexName(request.IndexName);
        BulkDescriptor descriptor = new BulkDescriptor(indexName);
        foreach (var item in request.Items)
        {
            descriptor
                .Index<TDocument>(opt => opt.Document(item.Document)
                    .Index(indexName)
                    .Id(item.DocumentId));
        }

        var ret = await _elasticClient.BulkAsync(descriptor, cancellationToken);
        return new SetResponse(ret);
    }

    public async Task<Response.DeleteResponse> DeleteDocumentAsync(
        DeleteDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        IDeleteRequest newRequest = new DeleteRequest(GetIndexName(request.IndexName), new Id(request.DocumentId));
        return new Response.DeleteResponse(await _elasticClient.DeleteAsync(newRequest, cancellationToken));
    }

    public async Task<Response.DeleteMultiResponse> DeleteMultiDocumentAsync(
        DeleteMultiDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        var ret = await _elasticClient.DeleteManyAsync(request.DocumentIds, GetIndexName(request.IndexName), cancellationToken);
        return new DeleteMultiResponse(ret);
    }

    /// <summary>
    /// Update the document
    /// only if the document exists
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDocument"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public async Task<UpdateResponse> UpdateDocumentAsync<TDocument>(
        UpdateDocumentRequest<TDocument> request,
        CancellationToken cancellationToken = default)
        where TDocument : class
    {
        if (request.Request.Document != null)
        {
            var ret = await _elasticClient.UpdateAsync<TDocument>(
                request.Request.DocumentId,
                opt => opt.Doc(request.Request.Document).Index(GetIndexName(request.IndexName)),
                cancellationToken);
            return new UpdateResponse(ret);
        }

        if (request.Request.PartialDocument != null)
        {
            IUpdateRequest<TDocument, object> newRequest =
                new UpdateRequest<TDocument, object>(GetIndexName(request.IndexName), request.Request.DocumentId)
                {
                    Doc = request.Request.PartialDocument
                };
            return new UpdateResponse(await _elasticClient.UpdateAsync(newRequest, cancellationToken));
        }

        throw new NotSupportedException("Document is error");
    }

    /// <summary>
    /// Update documents in batches
    /// only when the documents exist
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDocument"></typeparam>
    /// <returns></returns>
    public async Task<UpdateMultiResponse> UpdateMultiDocumentAsync<TDocument>(
        UpdateMultiDocumentRequest<TDocument> request,
        CancellationToken cancellationToken = default)
        where TDocument : class
    {
        var indexName = GetIndexName(request.IndexName);
        BulkDescriptor descriptor = new BulkDescriptor(indexName);
        foreach (var item in request.Items)
        {
            descriptor
                .Update<TDocument>(opt => opt.Doc(item.Document)
                    .Index(indexName)
                    .Id(item.DocumentId));
        }

        var ret = await _elasticClient.BulkAsync(descriptor, cancellationToken);
        return new UpdateMultiResponse(ret);
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

    private string GetIndexName(string? indexName) => indexName ?? DefaultIndex;
}
