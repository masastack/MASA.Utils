namespace System;

public class EnumObject<TValue>
{
    public string Name { get; set; } = default!;

    public TValue Value { get; set; } = default!;
}
