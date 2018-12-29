using Microsoft.VisualStudio.TestTools.UnitTesting;
using PspDataLayer;
using PspDataLayer.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PspDataLayer.Tests
{
    [TestClass()]
    public class PspDataAdapterTests
    {
        [TestMethod()]
        public async Task GetDataAsyncTest()
        {
            try
            {
                ConfigurationManagerJSON config = new ConfigurationManagerJSON();
                PspDataAdapter dataAdapter = new PspDataAdapter { ConfigurationManager = config };
                string measLabel = "gujarat_thermal_mu";
                DateTime fromTime = DateTime.Now.AddDays(-10);
                DateTime toTime = DateTime.Now.AddDays(-1);
                Dictionary<string, List<DataPoint>> result = await dataAdapter.GetDataAsync(fromTime, toTime, measLabel);
                if (result[measLabel].Count == 0)
                {
                    Assert.Fail("No data was returned");
                }
                else
                {
                    if (!result[measLabel][0].Time.Day.Equals(fromTime.Day))
                    {
                        Assert.Fail("Start data point date was not the requested one");
                    }
                    if (!result[measLabel].Last().Time.Day.Equals(toTime.Day))
                    {
                        Assert.Fail("End data point date was not the requested one");
                    }
                }
            }
            catch (Exception e)
            {
                Assert.Fail($"PSP Data adapter get async data failed by throwing error - {e.Message}");
            }
        }
    }
}