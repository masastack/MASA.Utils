﻿namespace MASA.Utils.Data.Elasticsearch.Options.Document.Query;

public class QueryOptions<TDocument> : QueryBaseOptions<TDocument>
    where TDocument : class
{
    public int Skip { get; }

    public int Take { get; }

    public QueryOptions(string query, string defaultField, int skip, int take)
        : this(null, query, defaultField, skip, take)
    {
    }

    public QueryOptions(string? indexName, string query, string defaultField, int skip, int take)
        : base(indexName, query, defaultField)
    {
        Skip = skip;
        Take = take;
    }
}
