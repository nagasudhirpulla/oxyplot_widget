using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMUDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PMUDataLayer.Tests
{
    [TestClass()]
    public class DiscoverMeasurementTests
    {
        [TestMethod()]
        public void GetMeasTreeTest()
        {
            DiscoverMeasurement discoverMeasurement = new DiscoverMeasurement();
            XDocument res = discoverMeasurement.GetMeasTree("pdcAdmin", "p@ssw0rd");
        }
    }
}