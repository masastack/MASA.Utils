中 | [EN](README.md)

## Masa.Utils.Data.Elasticsearch

## 用例:

```c#
Install-Package Masa.Utils.Data.Elasticsearch
```

#### 基本用法:

使用Elasticsearch

``` C#
builder.Services.AddElasticsearch("es", "http://localhost:9200"); // 或者builder.Services.AddElasticsearchClient("es", "http://localhost:9200");
```

#### 创建索引：

``` C#
public async Task<string> CreateIndexAsync([FromServices] IMasaElasticClient client)
{
    string indexName = "user_index_1";
    await client.CreateIndexAsync(indexName);
}
```

#### 删除索引：

``` C#
public async Task<string> DeleteIndexAsync([FromServices] IMasaElasticClient client)
{
    string indexName = "user_index_1";
    await client.DeleteIndexAsync(indexName);
}
```

#### 根据别名删除索引：

``` C#
public async Task<string> DeleteIndexByAliasAsync([FromServices] IMasaElasticClient client)
{
    string alias = "userIndex";
    await client.DeleteIndexByAliasAsync(alias);
}
```

#### 绑定别名

``` C#
public async Task<string> BindAliasAsync([FromServices] IMasaElasticClient client)
{
    string indexName = "user_index_1";
    string indexName2 = "user_index_2";
    string alias = "userIndex";
    await client.BindAliasAsync(new BindAliasIndexOptions(alias, new[] { indexName, indexName2 });
}
```

#### 解除别名绑定

``` C#
public async Task<string> BindAliasAsync([FromServices] IMasaElasticClient client)
{
    string indexName = "user_index_1";
    string indexName2 = "user_index_2";
    string alias = "userIndex";
    await client.UnBindAliasAsync(new UnBindAliasIndexOptions(alias, new[] { indexName, indexName2 }));
}
```

> 更多方法请查看[IMasaElasticClient](./IMasaElasticClient.cs)
