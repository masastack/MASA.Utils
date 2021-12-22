namespace MASA.Utils.Data.Elasticsearch;

public interface IMasaElasticClient
{
    string DefaultIndex { get; }

    #region index manage

    Task<Response.ExistsResponse> IndexExistAsync(CancellationToken cancellationToken = default);

    Task<Response.ExistsResponse> IndexExistAsync(
        string indexName,
        CancellationToken cancellationToken = default);

    Task<Response.CreateIndexResponse> CreateIndexAsync(
        CreateIndexOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<Response.CreateIndexResponse> CreateIndexAsync(
        string indexName,
        CreateIndexOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<Response.DeleteIndexResponse> DeleteIndexAsync(
        DeleteIndexOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<Response.DeleteIndexResponse> DeleteIndexAsync(
        string indexName,
        DeleteIndexOptions? options = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region document manage

    Task<Response.CreateResponse> CreateDocumentAsync<TDocument>(
        TDocument document,
        string? documentId = null,
        CancellationToken cancellationToken = default) where TDocument : class;

    Task<Response.CreateResponse> CreateDocumentAsync<TDocument>(
        string indexName,
        TDocument document,
        string? documentId = null,
        CancellationToken cancellationToken = default) where TDocument : class;

    Task<Response.DeleteResponse> DeleteDocumentAsync(
        string documentId,
        CancellationToken cancellationToken = default);

    Task<Response.DeleteResponse> DeleteDocumentAsync(
        string indexName,
        string documentId,
        CancellationToken cancellationToken = default);

    #endregion
}
