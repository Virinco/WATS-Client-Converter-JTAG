using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using Virinco.WATS.Interface;
using System.IO;

namespace JTAGConverters
{
    [TestClass]
    public class ConverterTests : TDM
    {
        [TestMethod]
        public void SetupClient()
        {
            SetupAPI(null, "", "Test", true);
            RegisterClient("your wats", "username", "password");
            InitializeAPI(true);
        }

        [TestMethod]
        public void TestXJLogConverter()
        {
            InitializeAPI(true);
            XJLogConverter converter = new XJLogConverter();
            using (FileStream file = new FileStream(@"Examples\567567_14_29_24_17_06_2020_Pass.xjlog", FileMode.Open))
            {
                converter.ImportReport(this, file);
            }
        }
    }
}
