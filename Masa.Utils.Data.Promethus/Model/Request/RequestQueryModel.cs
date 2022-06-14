// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Promethus.Model;

public class RequestQueryModel
{
    public string? Query { get; set; }

    public string? Time { get; set; }

    public string? TimeOut { get; set; }
}

public class RequestQueryRangeModel
{
    public string? Query { get; set; }

    public string? Start { get; set; }

    public string? End { get; set; }

    public string? Step { get; set; }

    public string? TimeOut { get; set; }
}
