namespace Memolap.Core.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
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
            Assert.IsNotNull(result);
            Assert.AreSame(this.dimension, result.Dimension);
            Assert.AreEqual("Argentina", result.Object);
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
