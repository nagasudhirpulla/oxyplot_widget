using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMUDataLayer.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMUDataLayer.Config.Tests
{
    [TestClass()]
    public class ConfigurationManagerJSONTests
    {
        [TestMethod()]
        public void InitializeTest()
        {
            try
            {
                // partial json also accepted as default values are taken for absent props
                ConfigurationManagerJSON configuration = new ConfigurationManagerJSON();
                configuration.Initialize();                
            }
            catch (Exception)
            {
                Assert.Fail("Test failed as initialization code thrown error");
            }
            Assert.AreEqual(1, 1);
        }

        [TestMethod()]
        public void SaveTest()
        {
            try
            {
                // partial json also accepted as default values are taken for absent props
                ConfigurationManagerJSON configuration = new ConfigurationManagerJSON();
                configuration.Save();
            }
            catch (Exception)
            {
                Assert.Fail("Test failed as saving code thrown error");
            }
            Assert.AreEqual(1, 1);
        }
    }
}