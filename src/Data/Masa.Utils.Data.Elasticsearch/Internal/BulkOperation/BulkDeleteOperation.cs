namespace Masa.Utils.Data.Elasticsearch.Internal.BulkOperation;

public class BulkDeleteOperation : BulkOperationBase
{
    public BulkDeleteOperation(Id id) => Id = id;

    protected override object GetBody() => null!;

    protected override Type ClrType { get; } = default!;

    protected override string Operation => "delete";
}
