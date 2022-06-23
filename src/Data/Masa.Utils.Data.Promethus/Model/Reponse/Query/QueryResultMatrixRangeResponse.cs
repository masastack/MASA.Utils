// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Promethus.Model;

public class QueryResultMatrixRangeResponse
{
    public IDictionary<string, object>? Metric { get; set; }

    public IEnumerable<object[]>? Values { get; set; }
}
