namespace Memolap.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TupleStoreBlockTests
    {
        [TestMethod]
        public void CreateTupleStoreBlock()
        {
            var block = new TupleStoreBlock<double>(6);

            Assert.AreEqual(1024, block.Size);
            Assert.AreEqual(0, block.Position);
            Assert.AreEqual(6, block.NDimensions);
        }

        [TestMethod]
        public void AddValuesData()
        {
            var block = new TupleStoreBlock<double>(6);

            block.Add(new ushort[] { 1, 2, 3, 4, 5, 6 }, 1.2);

            Assert.AreEqual(1024, block.Size);
            Assert.AreEqual(1, block.Position);
            Assert.AreEqual(6, block.NDimensions);

            for (int k = 0; k < 6; k++)
                Assert.AreEqual(k + 1, block.Values[k]);
        }
    }
}
