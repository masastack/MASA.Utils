// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

[assembly: InternalsVisibleTo("Masa.Utils.Data.Promethus.Test")]

namespace Masa.Utils.Data.Promethus;

internal class MasaPromethusClient : IMasaPromethusClient
{
    public Task<ResponseExemplarResultModel> ExemplarQueryAsync(RequestQueryModel query)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseLabelResultModel> LabelsQueryAsync(RequestMetaDataQueryModel query)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseLabelResultModel> LabelValuesQueryAsync(RequestMetaDataQueryModel query)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseQueryResultCommonModel> QueryAsync(RequestQueryModel query)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseQueryResultCommonModel> QueryRangeAsync(RequestQueryRangeModel query)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseSerieResultModel> SeriesAsync(RequestMetaDataQueryModel query)
    {
        throw new NotImplementedException();
    }
}
