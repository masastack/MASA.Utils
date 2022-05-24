// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Development.Dapr.Internal;

internal class EnvironmentUtils
{
    public static void TryAdd(string environment, Func<string?> func)
    {
        var value = Environment.GetEnvironmentVariable(environment);
        if (value == null)
        {
            Environment.SetEnvironmentVariable(environment, func.Invoke());
        }
    }
}
