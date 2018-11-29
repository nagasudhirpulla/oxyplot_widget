using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMUDataLayer;
using PMUDataLayer.Config;
using PMUDataLayer.DataExchangeClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMUDataLayer.Tests
{
    [TestClass()]
    public class HistoryDataAdapterTests
    {
        [TestMethod()]
        public void HistoryDataAdapterTest()
        {
            ConfigurationManagerJSON configuration;
            HistoryDataAdapter adapter;

            //constructor creation test
            try
            {
                configuration = new ConfigurationManagerJSON();
                adapter = new HistoryDataAdapter(configuration);
            }
            catch (Exception)
            {
                Assert.Fail("PMU History Data adapter initialization by constructor failed");
            }

            // initialization after construction test
            try
            {
                configuration = new ConfigurationManagerJSON();
                adapter = new HistoryDataAdapter();
                adapter.Initialize(configuration);
            }
            catch (Exception)
            {
                Assert.Fail("PMU History Data adapter initialization after construction failed");
            }
        }

        [TestMethod()]
        public async Task GetDataAsyncTestAsync()
        {
            try
            {
                HistoryDataAdapter adapter = new HistoryDataAdapter(new ConfigurationManagerJSON());
                DateTime startTime = DateTime.Now.AddMinutes(-2);
                DateTime endTime = startTime.AddMinutes(1);
                List<int> measIds = new List<int> { 4924 };
                Dictionary<object, List<PMUDataStructure>> res = await adapter.GetDataAsync(startTime, endTime, measIds, true, false, 25);

                // check if start time is expected
                DateTime dataTime = res.Values.ElementAt(0)[0].TimeStamp;
                // convert the time from utc to local
                dataTime = DateTime.SpecifyKind((TimeZoneInfo.ConvertTime(dataTime, TimeZoneInfo.Utc, TimeZoneInfo.Local)), DateTimeKind.Local);
                TimeSpan timeDiff = dataTime - startTime;
                Assert.AreEqual(timeDiff.TotalMilliseconds, 0);

                // check of result has keys with count same as measIds
                Assert.AreEqual(measIds.Count, res.Keys.Count);

                // since we are testing for full resolution, check if we have numSecs*25 samples
                Assert.AreEqual(res.Values.ElementAt(0).Count, Math.Floor((endTime - startTime).TotalSeconds) * 25);
            }
            catch (Exception e)
            {
                Assert.Fail($"PMU History Data adapter get async data failed by throwing error - {e.Message}");
            }
        }
    }
}