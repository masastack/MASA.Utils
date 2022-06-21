// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Promethus.Model;

public class SerieResultResponse : ResultBaseResponse
{
    public IEnumerable<IDictionary<string, string>>? Data { get; set; }
}
