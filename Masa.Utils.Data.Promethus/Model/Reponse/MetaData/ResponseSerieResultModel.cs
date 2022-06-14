// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Promethus.Model;

public class ResponseSerieResultModel : ResponseResultBaseModel
{
    public IEnumerable<IDictionary<string, object>>? Data { get; set; }
}
