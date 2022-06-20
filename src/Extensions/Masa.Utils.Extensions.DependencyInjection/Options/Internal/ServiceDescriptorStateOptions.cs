// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection.Options;

/// <summary>
/// State configuration for storing service information and whether a service needs to be replaced
/// </summary>
internal class ServiceDescriptorStateOptions : ServiceDescriptorOptions
{
    public bool? ReplaceServices { get; set; }

    public ServiceDescriptorStateOptions(Type serviceType, Type implementationType, ServiceLifetime lifetime, bool autoFire)
        : base(serviceType, implementationType, lifetime, autoFire)
    {
    }
}
