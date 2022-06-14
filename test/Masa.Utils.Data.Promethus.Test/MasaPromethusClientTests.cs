using Masa.Utils.Data.Promethus.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Masa.Utils.Data.Promethus.Test
{
    [TestClass]
    public class MasaPromethusClientTests
    {
        private MasaPromethusClient _client;

        [TestMethod]
        public async Task TestSeriesAsync()
        {
            var data = await _client.SeriesAsync(new Model.RequestMetaDataQueryModel
            {
                Match = null,

            });
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public async Task TestQueryAsync()
        {
            var data = await _client.SeriesAsync(new Model.RequestMetaDataQueryModel
            {
                Match = null,

            });
            Assert.IsNotNull(data);
        }

        [TestMethod]
        [DataRow()]
        public async Task TestExemplarQueryAsync(string time, string timeOut, string query)
        {
            var param = new Model.RequestQueryModel
            {
                Query = query,
                Time = time,
                TimeOut = timeOut
            };
            var data = await _client.ExemplarQueryAsync(param);
            if (string.IsNullOrEmpty(query))
                Assert.AreEqual(ResultStatuses.Error, data.Status);
        }

        [TestMethod]
        public async Task TestLabelsQueryAsync()
        {
            await Task.CompletedTask;
        }

        [TestMethod]
        public async Task TestLabelValuesQueryAsync()
        {
            await Task.CompletedTask;
        }


        [TestMethod]
        public async Task TestQueryRangeAsync()
        {
            await Task.CompletedTask;
        }
    }
}
