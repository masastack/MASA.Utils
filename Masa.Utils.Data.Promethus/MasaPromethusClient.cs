// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

[assembly: InternalsVisibleTo("Masa.Utils.Data.Promethus.Test")]

namespace Masa.Utils.Data.Promethus;

internal class MasaPromethusClient : IMasaPromethusClient
{
    private readonly ICallerProvider _caller;
    private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public MasaPromethusClient(ICallerProvider caller)
    {
        _caller = caller;
    }

    public async Task<ResponseExemplarResultModel> ExemplarQueryAsync(RequestQueryExemplarModel query)
    {
        return await QueryDataAsync<ResponseExemplarResultModel>("/api/v1/query_exemplars", query);
    }

    public async Task<ResponseLabelResultModel> LabelsQueryAsync(RequestMetaDataQueryModel query)
    {
        return await QueryDataAsync<ResponseLabelResultModel>("/api/v1/labels", query);
    }

    public async Task<ResponseLabelResultModel> LabelValuesQueryAsync(RequestLableValueQueryModel query)
    {
        var name = query.Lable;
        query.Lable = null;
        return await QueryDataAsync<ResponseLabelResultModel>($"/api/v1/label/{name}/values", query);
    }

    public async Task<ResponseQueryResultCommonModel> QueryAsync(RequestQueryModel query)
    {
        return await QueryDataAsync<ResponseQueryResultCommonModel>("/api/v1/query", query);
    }

    public async Task<ResponseQueryResultCommonModel> QueryRangeAsync(RequestQueryRangeModel query)
    {
        return await QueryDataAsync<ResponseQueryResultCommonModel>("/api/v1/query_range", query);
    }

    public async Task<ResponseSerieResultModel> SeriesAsync(RequestMetaDataQueryModel query)
    {
        return await QueryDataAsync<ResponseSerieResultModel>("/api/v1/series", query);
    }

    private void CheckOption()
    {
        if (_jsonSerializerOptions.Converters.Any(t => t.GetType() == typeof(JsonStringEnumConverter)))
            return;
        else
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    private async Task<T> QueryDataAsync<T>(string url, object data) where T : ResponseResultBaseModel
    {
        var str = await _caller.GetAsync(url, data);
        if (string.IsNullOrEmpty(str))
            return default!;

        CheckOption();
        var baseResult = JsonSerializer.Deserialize<T>(str, _jsonSerializerOptions);

        if (baseResult == null || baseResult.Status != ResultStatuses.Success)
        {
            return baseResult ?? default!;
        }

        if (typeof(T) == typeof(ResponseQueryResultCommonModel))
        {
            var result = baseResult as ResponseQueryResultCommonModel;
            if (result == null || result.Data == null)
                return baseResult;
            switch (result.Data.ResultType)
            {
                case ResultTypes.Matrix:
                    {
                        var temp = JsonSerializer.Serialize(result.Data.Result, _jsonSerializerOptions);
                        result.Data.Result = JsonSerializer.Deserialize<MatrixRangeModel[]>(temp, _jsonSerializerOptions);
                        if (result.Data.Result != null && result.Data.Result.Any())
                        {
                            foreach (MatrixRangeModel item in result.Data.Result)
                            {
                                if (item.Values == null || !item.Values.Any())
                                    continue;
                                var array = item.Values.ToArray();
                                int i = 0, max = array.Length - 1;
                                do
                                {
                                    array[i] = ConvertJsonToObjValue(array[i]);
                                    i++;
                                }
                                while (max - i >= 0);
                                item.Values = array;
                            }
                        }
                        return result as T ?? default!;
                    }
                case ResultTypes.Vector:
                    {
                        var temp = JsonSerializer.Serialize(result.Data.Result, _jsonSerializerOptions);
                        result.Data.Result = JsonSerializer.Deserialize<InstantVectorModel[]>(temp, _jsonSerializerOptions);
                        if (result.Data.Result != null && result.Data.Result.Any())
                        {
                            foreach (InstantVectorModel item in result.Data.Result)
                            {
                                item.Value = ConvertJsonToObjValue(item.Value);
                            }
                        }
                        return result as T ?? default!;
                    }
                default:
                    {
                        if (result.Data.Result != null && result.Data.Result.Any())
                        {
                            result.Data.Result = ConvertJsonToObjValue(result.Data.Result);
                        }
                    }
                    break;
            }
        }

        return baseResult;
    }

    private static object[] ConvertJsonToObjValue(object[]? values)
    {
        if (values == null || values.Length - 2 < 0)
            return default!;

        return new object[] { Convert.ToDouble(values[0]), values[1]?.ToString() ?? default! };
    }
}
