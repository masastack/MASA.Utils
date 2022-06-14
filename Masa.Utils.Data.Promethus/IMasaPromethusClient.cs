// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Promethus;

internal interface IMasaPromethusClient
{
    Task<ResponseQueryResultCommonModel> QueryAsync(RequestQueryModel query);

    Task<ResponseQueryResultCommonModel> QueryRangeAsync(RequestQueryRangeModel query);

    Task<ResponseSerieResultModel> SeriesAsync(RequestMetaDataQueryModel query);

    Task<ResponseLabelResultModel> LabelsQueryAsync(RequestMetaDataQueryModel query);

    Task<ResponseLabelResultModel> LabelValuesQueryAsync(RequestMetaDataQueryModel query);

    Task<ResponseExemplarResultModel> ExemplarQueryAsync(RequestQueryModel query);
}
