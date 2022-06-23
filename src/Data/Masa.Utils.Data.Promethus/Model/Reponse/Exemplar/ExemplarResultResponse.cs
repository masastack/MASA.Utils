// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Promethus.Model;

public class ExemplarResultResponse : ResultBaseResponse
{
    public IEnumerable<ExemplarDataModel>? Data { get; set; }
}

public class ExemplarModel
{
    public IDictionary<string, object>? Labels { get; set; }

    public string? Value { get; set; }

    public float TimeStamp { get; set; }
}

public class ExemplarDataModel
{
    public IDictionary<string, object>? SeriesLabels { get; set; }

    public IEnumerable<ExemplarModel>? Exemplars { get; set; }
}
