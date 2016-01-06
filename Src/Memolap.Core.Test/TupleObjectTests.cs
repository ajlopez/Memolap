namespace Memolap.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TupleObjectTests
    {
        private DataSet<int> set;

        [TestInitialize]
        public void Setup()
        {
            this.set = new DataSet<int>("Test");
            this.set.CreateDimension("Country");
            this.set.CreateDimension("Category");
            this.set.CreateDimension("Product");
        }

        [TestMethod]
        public void HasValue()
        {
            var tuple = this.set.AddData(
                new Dictionary<string, object>()
                {
                    { "Country", "Argentina" },
                    { "Category", "Beverages" },
                    { "Product", "Beer" }
                },
                100);

            Assert.IsTrue(tuple.HasValue("Country", "Argentina"));
            Assert.IsTrue(tuple.HasValue("Category", "Beverages"));
            Assert.IsTrue(tuple.HasValue("Product", "Beer"));

            Assert.IsFalse(tuple.HasValue("Province", "Buenos Aires"));
        }

        [TestMethod]
        public void Clone()
        {
            var tuple = this.set.AddData(
                new Dictionary<string, object>()
                {
                    { "Country", "Argentina" },
                    { "Category", "Beverages" },
                    { "Product", "Beer" }
                },
                100);

            var clone = tuple.Clone();

            Assert.IsTrue(clone.HasValue("Country", "Argentina"));
            Assert.IsTrue(clone.HasValue("Category", "Beverages"));
            Assert.IsTrue(clone.HasValue("Product", "Beer"));

            Assert.IsFalse(clone.HasValue("Province", "Buenos Aires"));
        }
    }
}
