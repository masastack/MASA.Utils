// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Promethus.Model;

public class RequestLableValueQueryModel: RequestMetaDataQueryModel
{
    public string Lable { get; set; } = "__name__";
}
