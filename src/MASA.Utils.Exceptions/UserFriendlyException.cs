namespace System;

public class UserFriendlyException : Exception
{
    public UserFriendlyException(string message)
        : base(message)
    {
    }
}
