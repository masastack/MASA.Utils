// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Elasticsearch.Response;

public class MappingResponse
{
    public string? Name { get; set; }

    public string? DataType { get; set; }

    public bool? IsKeyword { get; set; }

    public int? MaxLenth { get; set; }
}
