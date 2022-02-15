namespace System;

public class MasaException : Exception
{
    public MasaException()
    {
    }

    public MasaException(string message)
        : base(message)
    {
    }

    public MasaException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public MasaException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }
}
