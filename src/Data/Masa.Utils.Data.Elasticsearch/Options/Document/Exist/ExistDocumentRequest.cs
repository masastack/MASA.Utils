namespace Masa.Utils.Data.Elasticsearch.Options.Document.Exist;

public class ExistDocumentRequest : DocumentOptions
{
    public string DocumentId { get; set; }

    public ExistDocumentRequest(string indexName, string documentId) : base(indexName)
        => DocumentId = documentId;
}
