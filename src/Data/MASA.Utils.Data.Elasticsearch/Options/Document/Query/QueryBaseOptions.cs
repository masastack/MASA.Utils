namespace MASA.Utils.Data.Elasticsearch.Options.Document.Query;

public class QueryBaseOptions
{
    public string? IndexName { get; }

    public string[] Fields { get; }

    public string Query { get; }

    public Operator Operator { get; private set; }

    public QueryBaseOptions(string[] fields, string query, Operator @operator = Operator.Or)
        : this(null, fields, query, @operator)
    {
    }

    public QueryBaseOptions(string? indexName, string[] fields, string query, Operator @operator = Operator.Or)
    {
        IndexName = indexName;
        Fields = fields;
        Query = query;
        Operator = @operator;
    }

    public QueryBaseOptions UseOperator(Operator @operator)
    {
        Operator = @operator;
        return this;
    }
}
