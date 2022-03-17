﻿namespace Masa.Utils.Data.EntityFrameworkCore.Filters;

public class DataFilter : IDataFilter
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, object> _filters;

    public DataFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _filters = new();
    }

    public IDisposable Enable<TFilter>() where TFilter : class
        => GetFilter<TFilter>().Enable();

    public IDisposable Disable<TFilter>() where TFilter : class
        => GetFilter<TFilter>().Disable();

    public bool IsEnabled<TFilter>() where TFilter : class
        => GetFilter<TFilter>().IsEnabled;

    private DataFilter<TFilter> GetFilter<TFilter>()
        where TFilter : class
    {
        return (_filters.GetOrAdd(
            typeof(TFilter),
            _ => _serviceProvider.GetRequiredService<DataFilter<TFilter>>()
        ) as DataFilter<TFilter>)!;
    }
}

public class DataFilter<TFilter> where TFilter : class
{
    private readonly AsyncLocal<bool> _filter;

    public DataFilter()
    {
        _filter = new AsyncLocal<bool> { Value = true };
    }

    public bool IsEnabled => _filter.Value;

    public IDisposable Enable()
    {
        if (IsEnabled)
            return NullDisposable.Instance;

        _filter.Value = true;

        return new DisposeAction(() => Disable());
    }

    public IDisposable Disable()
    {
        if (!IsEnabled)
            return NullDisposable.Instance;

        _filter.Value = false;

        return new DisposeAction(() => Enable());
    }

}