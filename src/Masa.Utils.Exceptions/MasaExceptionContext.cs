// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace System;

public class MasaExceptionContext
{
    public Exception Exception { get; set; }

    public HttpContext HttpContext { get; }

    public bool ExceptionHandled { get; set; }

    public int StatusCode { get; set; }

    public string? Message { get; set; }

    public string ContentType { get; set; }

    internal MasaExceptionContext(Exception exception, HttpContext httpContext)
    {
        Exception = exception;
        HttpContext = httpContext;
        ExceptionHandled = false;
        ContentType = Constant.DEFAULT_HTTP_CONTENT_TYPE;
    }

    public void ToResult(
        string message,
        int statusCode = (int)MasaHttpStatusCode.UserFriendlyException,
        string contentType = Constant.DEFAULT_HTTP_CONTENT_TYPE)
    {
        Message = message;
        StatusCode = statusCode;
        ExceptionHandled = true;
        ContentType = contentType;
    }
}
