namespace Masa.Utils.Data.Mapping.Internal;

internal class InvokeBuilder
{
    private static readonly MethodInfo NewConfigMethodInfo;
    private static readonly Type TypeAdapterConfigType;

    static InvokeBuilder()
    {
        var typeAdapterSetterExpandType = typeof(TypeAdapterSetterExpand);
        NewConfigMethodInfo = typeAdapterSetterExpandType.GetMethod(nameof(TypeAdapterSetterExpand.NewConfigByConstructor))!;
        TypeAdapterConfigType = typeof(TypeAdapterConfig);
    }

    internal delegate TypeAdapterSetter MethodExecutor(TypeAdapterConfig target, object parameter);

    public static MethodExecutor Build(
        Type sourceType,
        Type destinationType)
    {
        var methodInfo = NewConfigMethodInfo.MakeGenericMethod(sourceType, destinationType);

        ParameterExpression[] parameters =
        {
            Expression.Parameter(TypeAdapterConfigType, "adapterConfigParameter"),
            Expression.Parameter(typeof(object), "constructorInfoParameter")
        };
        var newConfigMethodCall = Expression.Call(
            null,
            methodInfo,
            parameters
        );

        var lambda = Expression.Lambda<MethodExecutor>(newConfigMethodCall, parameters);
        return lambda.Compile();
    }
}
