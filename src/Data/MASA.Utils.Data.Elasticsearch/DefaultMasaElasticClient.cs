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

    public async Task<Response.ExistsResponse> IndexExistAsync(
        string? indexName = null,
        CancellationToken cancellationToken = default)
    {
        IIndexExistsRequest request = new IndexExistsRequest(GetIndices(indexName));
        return new Response.ExistsResponse(await _elasticClient.Indices.ExistsAsync(request, cancellationToken));
    }

    public async Task<Response.Index.CreateIndexResponse> CreateIndexAsync(
        string? indexName = null,
        CreateIndexOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ICreateIndexRequest request = new CreateIndexRequest(GetIndexName(indexName));
        if (options != null)
        {
            request.Settings = options.IndexSettings;
            request.Aliases = options.Aliases;
            request.Mappings = options.Mappings;
        }
        return new Response.Index.CreateIndexResponse(await _elasticClient.Indices.CreateAsync(request, cancellationToken));
    }

    public async Task<Response.Index.DeleteIndexResponse> DeleteIndexAsync(
        string? indexName = null,
        CancellationToken cancellationToken = default)
    {
        IDeleteIndexRequest request = new DeleteIndexRequest(GetIndices(indexName));
        return new Response.Index.DeleteIndexResponse(await _elasticClient.Indices.DeleteAsync(request, cancellationToken));
    }

    public async Task<Response.Index.DeleteIndexResponse> DeleteMultiIndexAsync(
        string[] indexNames,
        CancellationToken cancellationToken = default)
    {
        BulkAliasDescriptor request = new BulkAliasDescriptor();

        foreach (var indexName in indexNames)
        {
            request.RemoveIndex(opt => opt.Index(indexName));
        }

        return new Response.Index.DeleteIndexResponse(await _elasticClient.Indices.BulkAliasAsync(request, cancellationToken));
    }

    public async Task<Response.Index.DeleteIndexResponse> DeleteIndexByAliasAsync(
        string alias,
        CancellationToken cancellationToken = default)
    {
        var ret = await GetIndexByAliasAsync(alias, cancellationToken);
        if (ret.IsValid)
        {
            return await DeleteMultiIndexAsync(ret.IndexNames, cancellationToken);
        }

        return new MASA.Utils.Data.Elasticsearch.Response.Index.DeleteIndexResponse(ret.Message);
    }

    public async Task<Response.Index.GetIndexResponse> GetAllIndexAsync(CancellationToken cancellationToken)
    {
        ICatIndicesRequest request = new CatIndicesRequest();
        var ret = await _elasticClient.Cat.IndicesAsync(request, cancellationToken);
        return new Response.Index.GetIndexResponse(ret);
    }

    public async Task<Response.Index.GetIndexByAliasResponse> GetIndexByAliasAsync(string alias, CancellationToken cancellationToken)
    {
        ICatIndicesRequest request = new CatIndicesRequest(alias);
        var ret = await _elasticClient.Cat.IndicesAsync(request, cancellationToken);
        return new Response.Index.GetIndexByAliasResponse(ret);
    }

    #endregion

    #region alias manage

    public async Task<MASA.Utils.Data.Elasticsearch.Response.Alias.GetAliasResponse> GetAllAliasAsync(
        CancellationToken cancellationToken = default)
    {
        Func<CatAliasesDescriptor, ICatAliasesRequest>? selector = null;
        var ret = await _elasticClient.Cat.AliasesAsync(selector, cancellationToken);
        return new MASA.Utils.Data.Elasticsearch.Response.Alias.GetAliasResponse(ret);
    }

    public async Task<MASA.Utils.Data.Elasticsearch.Response.Alias.GetAliasResponse> GetAliasByIndexAsync(
        string? indexName = null,
        CancellationToken cancellationToken = default)
    {
        IGetAliasRequest request = new GetAliasRequest(GetIndices(indexName));
        var ret = await _elasticClient.Indices.GetAliasAsync(request, cancellationToken);
        return new MASA.Utils.Data.Elasticsearch.Response.Alias.GetAliasResponse(ret);
    }

    public async Task<MASA.Utils.Data.Elasticsearch.Response.Alias.BulkAliasResponse> BindAliasAsync(
        BindAliasIndexOptions options,
        CancellationToken cancellationToken = default)
    {
        BulkAliasDescriptor request = new BulkAliasDescriptor();
        var indexNames = options.IndexNames ?? new[] {DefaultIndex};
        foreach (var indexName in indexNames)
        {
            request.Add(opt => opt.Aliases(options.Alias).Index(indexName));
        }

        var ret = await _elasticClient.Indices.BulkAliasAsync(request, cancellationToken);
        return new MASA.Utils.Data.Elasticsearch.Response.Alias.BulkAliasResponse(ret);
    }

    public async Task<MASA.Utils.Data.Elasticsearch.Response.Alias.BulkAliasResponse> UnBindAliasAsync(
        UnBindAliasIndexOptions options,
        CancellationToken cancellationToken = default)
    {
        BulkAliasDescriptor request = new BulkAliasDescriptor();
        var indexNames = options.IndexNames ?? new[] {DefaultIndex};
        foreach (var indexName in indexNames)
        {
            request.Remove(opt => opt.Aliases(options.Alias).Index(indexName));
        }

        var ret = await _elasticClient.Indices.BulkAliasAsync(request, cancellationToken);
        return new MASA.Utils.Data.Elasticsearch.Response.Alias.BulkAliasResponse(ret);
    }

    #endregion

    #region document manage

    public async Task<Response.ExistsResponse> DocumentExistsAsync(
        ExistDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        var newRequest = new DocumentExistsRequest(GetIndex(request.IndexName), request.DocumentId);
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
            ? new CreateRequest<TDocument>(GetIndex(request.IndexName), new Id(request.Request.DocumentId))
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
    public async Task<CreateMultiResponse> CreateMultiDocumentAsync<TDocument>(
        CreateMultiDocumentRequest<TDocument> request,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var indexName = GetIndex(request.IndexName);
        BulkDescriptor descriptor = new BulkDescriptor(indexName);
        foreach (var item in request.Items)
        {
            descriptor
                .Create<TDocument>(opt => opt.Document(item.Document)
                    .Index(indexName)
                    .Id(item.DocumentId));
        }

        var ret = await _elasticClient.BulkAsync(descriptor, cancellationToken);
        return new CreateMultiResponse(ret);
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
        var indexName = GetIndex(request.IndexName);
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
        IDeleteRequest newRequest = new DeleteRequest(GetIndex(request.IndexName), new Id(request.DocumentId));
        return new Response.DeleteResponse(await _elasticClient.DeleteAsync(newRequest, cancellationToken));
    }

    public async Task<DeleteMultiResponse> DeleteMultiDocumentAsync(
        DeleteMultiDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        var ret = await _elasticClient.DeleteManyAsync(request.DocumentIds, GetIndex(request.IndexName), cancellationToken);
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
                opt => opt.Doc(request.Request.Document).Index(GetIndex(request.IndexName)),
                cancellationToken);
            return new UpdateResponse(ret);
        }

        IUpdateRequest<TDocument, object> newRequest =
            new UpdateRequest<TDocument, object>(GetIndex(request.IndexName), request.Request.DocumentId)
            {
                Doc = request.Request.PartialDocument!
            };
        return new UpdateResponse(await _elasticClient.UpdateAsync(newRequest, cancellationToken));
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
        var indexName = GetIndex(request.IndexName);
        BulkDescriptor descriptor = new BulkDescriptor(indexName);
        foreach (var item in request.Items)
        {
            if (item.Document != null)
            {
                descriptor
                    .Update<TDocument>(opt => opt.Doc(item.Document)
                        .Index(indexName)
                        .Id(item.DocumentId));
            }

            descriptor
                .Update<TDocument, object>(opt => opt.Doc(item.PartialDocument!)
                    .Index(indexName)
                    .Id(item.DocumentId));
        }

        var ret = await _elasticClient.BulkAsync(descriptor, cancellationToken);
        return new UpdateMultiResponse(ret);
    }

    public async Task<Response.GetResponse<TDocument>> GetAsync<TDocument>(
        GetDocumentRequest request,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        IGetRequest newRequest = new GetRequest(GetIndex(request.IndexName), request.Id);
        return new Response.GetResponse<TDocument>(await _elasticClient.GetAsync<TDocument>(newRequest, cancellationToken));
    }

    public async Task<GetMultiResponse<TDocument>> GetMultiAsync<TDocument>(
        GetMultiDocumentRequest request,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var ret = (await _elasticClient.GetManyAsync<TDocument>(request.Id, GetIndex(request.IndexName), cancellationToken))?.ToList()
                  ?? new List<IMultiGetHit<TDocument>>();
        if (ret.Count == request.Id.Length)
        {
            return new GetMultiResponse<TDocument>(true, "success", ret);
        }

        return new GetMultiResponse<TDocument>(false, "Failed to get document");
    }

    public async Task<Response.SearchResponse<TDocument>> GetListAsync<TDocument>(
        QueryOptions<TDocument> options,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var ret = await QueryString(
            options.IndexName,
            options.Skip,
            options.Take,
            options,
            cancellationToken);
        return new Response.SearchResponse<TDocument>(ret);
    }

    public async Task<SearchPaginatedResponse<TDocument>> GetPaginatedListAsync<TDocument>(
        PaginatedOptions<TDocument> options,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var ret = await QueryString(
            options.IndexName,
            (options.Page - 1) * options.PageSize,
            options.PageSize,
            options,
            cancellationToken);
        return new SearchPaginatedResponse<TDocument>(options.PageSize, ret);
    }

    private Task<ISearchResponse<TDocument>> QueryString<TDocument>(
        string? indexName,
        int skip,
        int take,
        QueryBaseOptions<TDocument> queryBaseOptions,
        CancellationToken cancellationToken = default)
        where TDocument : class
    {
        return _elasticClient.SearchAsync<TDocument>(s => s
            .Index(GetIndex(indexName))
            .From(skip)
            .Size(take)
            .Query(q => q
                .QueryString(qs => GetQueryDescriptor(qs, queryBaseOptions))
            ), cancellationToken);
    }

    private static QueryStringQueryDescriptor<TDocument> GetQueryDescriptor<TDocument>(
        QueryStringQueryDescriptor<TDocument> queryDescriptor,
        QueryBaseOptions<TDocument> queryBaseOptions)
        where TDocument : class
    {
        queryDescriptor = queryDescriptor.Query(queryBaseOptions.Query);
        if (queryBaseOptions.DefaultField != null)
        {
            queryDescriptor.DefaultField(queryBaseOptions.DefaultField);
        }

        if (queryBaseOptions.Fields.Length > 0)
        {
            queryDescriptor.Fields(queryBaseOptions.Fields);
        }

        queryBaseOptions.Action?.Invoke(queryDescriptor);

        return queryDescriptor;
    }

    #endregion

    private string GetIndex(string? indexName) => indexName ?? DefaultIndex;

    private Indices GetIndices(string? indexName) => GetIndex(indexName);

    private IndexName GetIndexName(string? indexName) => GetIndex(indexName);
}
