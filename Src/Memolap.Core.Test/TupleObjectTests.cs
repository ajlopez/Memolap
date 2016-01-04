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
            var tuple = this.set.CreateTuple(
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
        public void SetValue()
        {
            var tuple = this.set.CreateTuple();
            tuple.SetValue("Country", "Argentina");
            Assert.IsTrue(tuple.HasValue("Country", "Argentina"));
            tuple.SetValue("Country", "Uruguay");
            Assert.IsTrue(tuple.HasValue("Country", "Uruguay"));
            Assert.IsFalse(tuple.HasValue("Country", "Argentina"));
        }

        [TestMethod]
        public void GetValues()
        {
            var tuple = this.set.CreateTuple();
            tuple.SetValue("Country", "Argentina");
            tuple.SetValue("Category", "Beverages");

            var values = tuple.GetValues().ToList();

            Assert.IsNotNull(values);
            Assert.AreEqual(3, values.Count);
            Assert.AreEqual("Argentina", values[0]);
            Assert.AreEqual("Beverages", values[1]);
            Assert.IsNull(values[2]);
        }

        [TestMethod]
        public void GetValueOnEmptyDimension()
        {
            var tuple = this.set.CreateTuple();
            Assert.IsNull(tuple.GetValue("Country"));

            tuple.GetValues().All(v => v == null);
        }
    }
}
