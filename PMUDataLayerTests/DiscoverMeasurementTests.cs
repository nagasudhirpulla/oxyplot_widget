using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMUDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMUDataLayer.Tests
{
    [TestClass()]
    public class DiscoverMeasurementTests
    {
        [TestMethod()]
        public void GetMeasTreeTest()
        {
            DiscoverMeasurement discoverMeasurement = new DiscoverMeasurement();
            string res = discoverMeasurement.GetMeasTree("pdcAdmin", "p@ssw0rd");
        }
    }
}