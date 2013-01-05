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
        [TestMethod]
        public void CreateDimension()
        {
            Engine engine = new Engine();
            var dimension = engine.CreateDimension("Country");

            Assert.IsNotNull(dimension);
            Assert.AreEqual("Country", dimension.Name);
        }

        [TestMethod]
        public void CreateDimensionWithSetName()
        {
            Engine engine = new Engine();
            var dimension = engine.CreateDimension("Country");

            Assert.IsNotNull(dimension);
            Assert.AreEqual("Country", dimension.Name);
            Assert.AreEqual("Countries", dimension.SetName);
        }

        [TestMethod]
        public void GetDimension()
        {
            Engine engine = new Engine();
            var dimension = engine.CreateDimension("Country");
            var result = engine.GetDimension("Country");

            Assert.IsNotNull(result);
            Assert.AreSame(dimension, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfDuplicatedDimension()
        {
            Engine engine = new Engine();
            engine.CreateDimension("Country");
            engine.CreateDimension("Country");
        }

        [TestMethod]
        public void GetUnknownDimension()
        {
            Engine engine = new Engine();
            Assert.IsNull(engine.GetDimension("Foo"));
        }

        [TestMethod]
        public void GetDimensions()
        {
            Engine engine = new Engine();
            engine.CreateDimension("Country");
            engine.CreateDimension("Product");
            engine.CreateDimension("Category");

            var result = engine.GetDimensions();

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
            Engine engine = new Engine();

            var result = engine.AddTuple(new Dictionary<string, object>()
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
            Engine engine = new Engine();

            engine.AddTuple(new Dictionary<string, object>()
            {
                { "Country", "Argentina" },
                { "Category", "Beverages" },
                { "Product", "Beer" }
            });

            engine.AddTuple(new Dictionary<string, object>()
            {
                { "Country", "Argentina" },
                { "Category", "Beverages" },
                { "Product", "Coke" }
            });

            Assert.AreEqual(2, engine.GetTupleCount());
        }
    }
}
