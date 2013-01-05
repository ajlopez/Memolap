namespace Memolap.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EngineTests
    {
        private Engine engine;

        [TestInitialize]
        public void Setup()
        {
            this.engine = new Engine();
        }

        [TestMethod]
        public void CreateDimension()
        {
            var dimension = this.engine.CreateDimension("Country");

            Assert.IsNotNull(dimension);
            Assert.AreEqual("Country", dimension.Name);
        }

        [TestMethod]
        public void CreateDimensionWithSetName()
        {
            var dimension = this.engine.CreateDimension("Country");

            Assert.IsNotNull(dimension);
            Assert.AreEqual("Country", dimension.Name);
            Assert.AreEqual("Countries", dimension.SetName);
        }

        [TestMethod]
        public void GetDimension()
        {
            var dimension = this.engine.CreateDimension("Country");
            var result = this.engine.GetDimension("Country");

            Assert.IsNotNull(result);
            Assert.AreSame(dimension, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfDuplicatedDimension()
        {
            this.engine.CreateDimension("Country");
            this.engine.CreateDimension("Country");
        }

        [TestMethod]
        public void GetUnknownDimension()
        {
            Assert.IsNull(this.engine.GetDimension("Foo"));
        }

        [TestMethod]
        public void GetDimensions()
        {
            this.engine.CreateDimension("Country");
            this.engine.CreateDimension("Product");
            this.engine.CreateDimension("Category");

            var result = this.engine.GetDimensions();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(c => c.Name == "Country"));
            Assert.IsTrue(result.Any(c => c.SetName == "Countries"));
            Assert.IsTrue(result.Any(c => c.Name == "Product"));
            Assert.IsTrue(result.Any(c => c.SetName == "Products"));
            Assert.IsTrue(result.Any(c => c.Name == "Category"));
            Assert.IsTrue(result.Any(c => c.SetName == "Categories"));
        }

        [TestMethod]
        public void AddTuple()
        {
            var result = this.engine.AddTuple(new Dictionary<string, object>()
            {
                { "Country", "Argentina" },
                { "Category", "Beverages" },
                { "Product", "Beer" }
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Size);
            Assert.IsTrue(result.HasValue("Country", "Argentina"));
            Assert.IsTrue(result.HasValue("Category", "Beverages"));
            Assert.IsTrue(result.HasValue("Product", "Beer"));
        }

        [TestMethod]
        public void GetTupleCount()
        {
            this.engine.AddTuple(new Dictionary<string, object>()
            {
                { "Country", "Argentina" },
                { "Category", "Beverages" },
                { "Product", "Beer" }
            });

            this.engine.AddTuple(new Dictionary<string, object>()
            {
                { "Country", "Argentina" },
                { "Category", "Beverages" },
                { "Product", "Coke" }
            });

            Assert.AreEqual(2, this.engine.GetTupleCount());
        }

        [TestMethod]
        public void GenerateTuplesAndGetCount()
        {
            GenerateTuples(3, 2);
            Assert.AreEqual(6, this.engine.GetTupleCount());
        }

        private void GenerateTuples(params int[] nvalues)
        {
            string[] dimensions = new string[nvalues.Length];

            for (var k = 0; k < nvalues.Length; k++)
                dimensions[k] = string.Format("Dimension{0}", k + 1);

            var dict = new Dictionary<string, object>();

            for (var k = 0; k < nvalues.Length; k++)
                for (var j = 0; j < nvalues[k]; j++)
                {
                    dict[dimensions[k]] = string.Format("Value {0}", j + 1);
                    if (k == nvalues.Length - 1)
                    {
                        var newdict = new Dictionary<string, object>();
                        foreach (var val in dict)
                            newdict[val.Key] = val.Value;
                        this.engine.AddTuple(newdict);
                    }
                }
        }
    }
}
