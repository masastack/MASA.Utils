// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Promethus.Test
{
    [TestClass]
    public class MasaPromethusClientTests
    {
        private IMasaPromethusClient _client;

        [TestInitialize]
        public void Initialize()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddPromethusClient("http://localhost:9090");
            _client = service.BuildServiceProvider().GetService<IMasaPromethusClient>();
        }

        [TestMethod]
        public async Task TestQueryAsync()
        {
            var result = await _client.QueryAsync(new QueryRequest
            {
                Query = "up"
            });
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Status, ResultStatuses.Success);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public async Task TestQueryVectorAsync()
        {
            var result = await _client.QueryAsync(new QueryRequest
            {
                Query = "up"
            });

            if (result.Data.Result != null)
            {
                var data = result.Data.Result as QueryResultInstantVectorResponse[];

            Assert.IsNotNull(data);
                Assert.IsNotNull(data[0].Metric);
                Assert.IsNotNull(data[0].Value);

                Assert.IsNotNull(data[0].Metric.Keys);
                Assert.AreEqual(2, data[0].Value.Length);
        }
        }

        [TestMethod]
        public async Task TestQueryRangeAsync()
        {
            var result = await _client.QueryRangeAsync(new QueryRangeRequest
            {
                Query = "up",
                Start = "2022-06-17T02:00:00.000Z",
                End = "2022-06-17T02:30:00.000Z",
                Step = "300s",
            });
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Status, ResultStatuses.Success);
            Assert.IsNotNull(result.Data);
            if (result.Data.ResultType == ResultTypes.Matrix)
            {
                var data = result.Data.Result as QueryResultMatrixRangeResponse[];
            Assert.IsNotNull(data);
                Assert.IsNotNull(data[0].Metric);
                Assert.IsNotNull(data[0].Values);
            }
        }

        [TestMethod]
        public async Task TestSeriesAsync()
        {
            var result = await _client.SeriesAsync(new MetaDataQueryRequest
            {
                Match = new string[] { "up" },
                Start = "2022-06-17T02:00:00.000Z",
                End = "2022-06-17T02:30:00.000Z"
            });
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Status, ResultStatuses.Success);
        }

        [TestMethod]
        public async Task TestLabelsQueryAsync()
        {
            var result = await _client.LabelsQueryAsync(default!);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Status, ResultStatuses.Success);
            Assert.IsTrue(result.Data.Count() > 0);
        }

        [TestMethod]
        public async Task TestLabelValuesQueryAsync()
        {
            var result = await _client.LabelValuesQueryAsync(new LableValueQueryRequest());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Status, ResultStatuses.Success);
            Assert.IsTrue(result.Data.Count() > 0);
        }


        [TestMethod]
        [DataRow()]
        public async Task TestExemplarQueryAsync()
        {
            var param = new QueryExemplarRequest
        {
                Query = "up",
                Start = "2022-06-17T02:00:00.000Z",
                End = "2022-06-17T02:30:00.000Z"
            };
            var result = await _client.ExemplarQueryAsync(param);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Status, ResultStatuses.Success);
        }
    }
}
