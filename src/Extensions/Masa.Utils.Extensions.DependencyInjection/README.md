[中](README.zh-CN.md) | EN

## Masa.Utils.Extensions.DependencyInjection

## Example:

````c#
Install-Package Masa.Utils.Extensions.DependencyInjection
````
### Usage:

````C#
services.AddAutoInject();
````

## Dependent interface:

* ISingletonDependency: registers a service whose lifecycle is Singleton
* IScopedDependency: registers a service whose lifecycle is Scoped
* ITransientDependency: registers services whose lifecycle is Transient
* IAutoFireDependency: is automatically triggered (used in combination with ISingletonDependency, IScopedDependency, and ITransientDependency to trigger a service acquisition operation after the service is automatically registered, only inheriting IAutoFireDependency does not work)

Example:

````c#
public interface IRepository<TEntity> : IScopedDependency
    where TEntity : class
{

}
````

> Because IRepository<TEntity> inherits IScopedDependency, the life cycle of IRepository<TEntity> will be Scoped

## Features:

* IgnoreInjection: Ignore injection, used to exclude sub-services from being injected automatically

## Methods:

* GetInstance<T>(): Get the instance of service T
* Any<T>(): whether service T exists, does not support generic services
* Any<T>(ServiceLifetime.Singleton): Whether there is a service T whose life cycle is Singleton, does not support generic services