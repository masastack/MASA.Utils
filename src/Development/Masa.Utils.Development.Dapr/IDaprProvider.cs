namespace Masa.Utils.Development.Dapr;

public interface IDaprProvider
{
    List<DaprRuntimeOptions> GetDaprList(string appId);

    bool IsExist(string appId);
}
