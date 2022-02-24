namespace Masa.Utils.Exceptions;

public class MasaExceptionHandlingOptions
{
    public bool CatchAllException { get; set; } = true;

    public Func<Exception, (Exception? OverrideException, bool ExceptionHandled)>? CustomExceptionHandler { get; set; }
}
