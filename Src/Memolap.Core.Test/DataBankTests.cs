using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Memolap.Core.Test
{
    [TestClass]
    public class DataBankTests
    {
        [TestMethod]
        public void CreateDataBank()
        {
            DataBank bank = new DataBank("Test");

            Assert.AreEqual("Test", bank.Name);
            Assert.IsNotNull(bank.Dimensions);
            Assert.AreEqual(0, bank.Dimensions.Count);
        }
    }
}
