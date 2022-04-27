// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Development.Dapr.Internal;

internal class DefaultOptions
{
    public static string DefaultAppId => ((Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName().Name ??
        throw new NotSupportedException("dapr appid is not empty")).Replace(".", Const.DEFAULT_APPID_DELIMITER);

    /// <summary>
    /// Appid suffix, the default is the current MAC address
    /// </summary>
    public static string DefaultAppidSuffix = NetworkUtils.GetPhysicalAddress();
}
