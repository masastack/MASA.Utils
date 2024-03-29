﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Exceptions;

public interface IMasaExceptionHandler
{
    void OnException(MasaExceptionContext context);
}
