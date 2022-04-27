// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Exceptions.Results;

public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object obj)
        : base(obj)
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
    }
}
