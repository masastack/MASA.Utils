// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Elasticsearch.Response;

public class SetResponse : BulkResponse
{
    public SetResponse(Nest.BulkResponse bulkResponse) : base(bulkResponse)
    {
    }
}
