[中](README.zh-CN.md) | EN

## Masa.Utils.Data.Promethus

[Promethus Http Api](https://www.prometheus.io/docs/prometheus/latest/querying/api/) Client Library

## Install:
```c#
Install-Package Masa.Utils.Data.Promethus
```

### Usage:

1. Inject services

```` C#
builder.Services.AddPromethusClient("http://127.0.0.1:9090");
````

2. Call

```C#
    public class SampleService{

        private IMasaPromethusClient _client;

        public SampleService(IMasaPromethusClient client)
        {
            _client=client;
        }

        public async Task QueryAsync()
        {
            var result=await _client.QueryAsync(...);
        }
    }
```

### Current suports:

- [query](https://www.prometheus.io/docs/prometheus/latest/querying/api/#instant-queries)
- [query_range](https://www.prometheus.io/docs/prometheus/latest/querying/api/#range-queries)
- [series](https://www.prometheus.io/docs/prometheus/latest/querying/api/#finding-series-by-label-matchers)
- [lables](https://www.prometheus.io/docs/prometheus/latest/querying/api/#getting-label-names)
- [lable value](https://www.prometheus.io/docs/prometheus/latest/querying/api/#querying-label-values)
- [exemplars](https://www.prometheus.io/docs/prometheus/latest/querying/api/#querying-exemplars)
