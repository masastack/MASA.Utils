// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Mapping;

public interface IMapping
{
    TDestination Map<TSource, TDestination>(TSource source, MapOptions? options = null);

    TDestination Map<TDestination>(object source, MapOptions? options = null);
}
