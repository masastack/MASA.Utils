namespace MASA.Utils.Data.Elasticsearch;

public interface IMasaElasticClient
{
    bool ExistDefaultIndex { get; }

    string DefaultIndex { get; }

    #region index manage

    Task<Response.ExistsResponse> IndexExistAsync(CancellationToken cancellationToken = default);

    Task<Response.ExistsResponse> IndexExistAsync(
        string indexName,
        CancellationToken cancellationToken = default);

    Task<Response.Index.CreateIndexResponse> CreateIndexAsync(
        CreateIndexOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<Response.Index.CreateIndexResponse> CreateIndexAsync(
        string indexName,
        CreateIndexOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<Response.Index.DeleteIndexResponse> DeleteIndexAsync(
        DeleteIndexOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<Response.Index.DeleteIndexResponse> DeleteIndexAsync(
        string indexName,
        DeleteIndexOptions? options = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region document manage

    Task<Response.ExistsResponse> DocumentExistsAsync(
        ExistDocumentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a new document
    /// only when the document does not exist
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDocument"></typeparam>
    /// <returns></returns>
    Task<Response.CreateResponse> CreateDocumentAsync<TDocument>(
        CreateDocumentRequest<TDocument> request,
        CancellationToken cancellationToken = default) where TDocument : class;

    /// <summary>
    /// Add new documents in batches
    /// only when the documents do not exist
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDocument"></typeparam>
    /// <returns></returns>
    Task<Response.CreateMultiResponse> CreateMultiDocumentAsync<TDocument>(
        CreateMultiDocumentRequest<TDocument> request,
        CancellationToken cancellationToken = default) where TDocument : class;

    /// <summary>
    /// Update or insert document
    /// Overwrite if it exists, add new if it does not exist
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDocument"></typeparam>
    /// <returns></returns>
    Task<SetResponse> SetDocumentAsync<TDocument>(
        SetDocumentRequest<TDocument> request,
        CancellationToken cancellationToken = default) where TDocument : class;

    Task<Response.DeleteResponse> DeleteDocumentAsync(
        DeleteDocumentRequest request,
        CancellationToken cancellationToken = default);

    Task<Response.DeleteMultiResponse> DeleteMultiDocumentAsync(
        DeleteMultiDocumentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the document
    /// only if the document exists
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDocument"></typeparam>
    /// <returns></returns>
    Task<UpdateResponse> UpdateDocumentAsync<TDocument>(
        UpdateDocumentRequest<TDocument> request,
        CancellationToken cancellationToken = default)
        where TDocument : class;

    /// <summary>
    /// Update documents in batches
    /// only when the documents exist
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TDocument"></typeparam>
    /// <returns></returns>
    Task<UpdateMultiResponse> UpdateMultiDocumentAsync<TDocument>(
        UpdateMultiDocumentRequest<TDocument> request,
        CancellationToken cancellationToken = default)
        where TDocument : class;

    Task<Response.SearchResponse<TDocument>> GetListAsync<TDocument>(
        QueryOptions options,
        CancellationToken cancellationToken = default) where TDocument : class;

    Task<SearchPaginatedResponse<TDocument>> GetPaginatedListAsync<TDocument>(
        PaginatedOptions options,
        CancellationToken cancellationToken = default) where TDocument : class;

    #endregion
}
