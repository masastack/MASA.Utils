// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Promethus;

public interface IMasaPromethusClient
{
    Task<QueryResultCommonResponse> QueryAsync(QueryRequest query);

    Task<QueryResultCommonResponse> QueryRangeAsync(QueryRangeRequest query);

    Task<SerieResultResponse> SeriesAsync(MetaDataQueryRequest query);

    Task<LabelResultResponse> LabelsQueryAsync(MetaDataQueryRequest query);

    Task<LabelResultResponse> LabelValuesQueryAsync(LableValueQueryRequest query);

    Task<ExemplarResultResponse> ExemplarQueryAsync(QueryExemplarRequest query);
}
