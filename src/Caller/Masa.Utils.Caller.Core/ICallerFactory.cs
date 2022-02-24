namespace Masa.Utils.Caller.Core;

public interface ICallerFactory
{
    ICallerProvider CreateClient();

    ICallerProvider CreateClient(string name);
}
