namespace Masa.Utils.Data.EntityFrameworkCore.Filters;

public interface IDataFilter
{
    IDisposable Enable<TFilter>() where TFilter : class;

    IDisposable Disable<TFilter>() where TFilter : class;

    bool IsEnabled<TFilter>() where TFilter : class;
}
