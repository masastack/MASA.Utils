中 | [EN](README.md)

## Masa.Utils.Extensions.DependencyInjection

## 用例:

```c#
Install-Package Masa.Utils.Extensions.DependencyInjection
```
### 用法:

```C#
services.AddAutoInject();
```

## 依赖接口:

* ISingletonDependency: 注册生命周期为Singleton的服务
* IScopedDependency: 注册生命周期为Scoped的服务
* ITransientDependency: 注册生命周期为Transient的服务
* IAutoFireDependency: 自动触发（与ISingletonDependency、IScopedDependency、ITransientDependency结合使用，在服务自动注册结束后触发一次获取服务操作，仅继承IAutoFireDependency不起作用）

示例:

```c#
public interface IRepository<TEntity> : IScopedDependency
    where TEntity : class
{

}
```

> 因IRepository<TEntity>继承IScopedDependency，所以会将IRepository<TEntity>的生命周期为Scoped

## 规则:

扫描程序集中继承ISingletonDependency、IScopedDependency、ITransientDependency的接口以及类，并为其自动注册服务

* 当继承的是类是接口时，其ServiceType是当前接口，其ImplementationType是当前接口的实现类
  > 如果当前接口有多个实现类，会被多次添加
* 当继承的类不是接口时，其ServiceType是当前类，其ImplementationType也是当前类
  > 默认支持级联扫描注册服务，当前类的子类也会被注册
  >
  > 抽象类不会被注册

## 特性:

* IgnoreInjection: 忽略注入，用于排除子服务被自动注入

## 方法:

* GetInstance<T>(): 获取服务T的实例
* Any<T>(): 是否存在服务T，不支持泛型服务
* Any<T>(ServiceLifetime.Singleton): 是否存在一个生命周期为Singleton的服务T，不支持泛型服务