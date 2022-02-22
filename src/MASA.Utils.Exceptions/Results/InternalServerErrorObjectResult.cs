namespace MASA.Utils.Exceptions.Results;

public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object obj)
        : base(obj)
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
    }
}
