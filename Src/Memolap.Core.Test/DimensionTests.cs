namespace Memolap.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DimensionTests
    {
        private Dimension dimension;

        [TestInitialize]
        public void Setup()
        {
            this.dimension = new Dimension("Country");
        }

        [TestMethod]
        public void GetValue()
        {
            var result = this.dimension.GetValue("Argentina");
            Assert.AreEqual("Argentina", this.dimension.GetValue(result));
        }

        [TestMethod]
        public void GetExistingValue()
        {
            var value = this.dimension.GetValue("Argentina");
            var result = this.dimension.GetValue("Argentina");
            Assert.AreSame(value, result);
        }
    }
}
