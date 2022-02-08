namespace MASA.Utils.Development.Dapr;

public interface IDaprProvider
{
    List<DaprRuntimeOptions> GetDaprList(string appId);
}
