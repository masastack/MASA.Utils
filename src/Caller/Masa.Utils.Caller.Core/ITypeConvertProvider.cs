namespace Masa.Utils.Caller.Core;

public interface ITypeConvertProvider
{
    /// <summary>
    /// Convert custom object to dictionary
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="TRequest">Support classes, anonymous objects</typeparam>
    /// <returns></returns>
    Dictionary<string, string> ConvertToDictionary<TRequest>(TRequest request) where TRequest : class;
}
