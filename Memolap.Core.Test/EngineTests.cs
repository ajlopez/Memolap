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

        [TestMethod]
        public void GetDimensionOneTuples()
        {
            GenerateTuples(3, 4, 5);
            Assert.AreEqual(60, this.engine.GetTupleCount());
            Assert.AreEqual(20, this.engine.GetTuples(new Dictionary<string, object>() { { "Dimension1", "Value 1" } }).Count());
        }

        [TestMethod]
        public void GetDimensionOneManyTuples()
        {
            GenerateTuples(3, 40, 50);
            Assert.AreEqual(6000, this.engine.GetTupleCount());
            Assert.AreEqual(2000, this.engine.GetTuples(new Dictionary<string, object>() { { "Dimension1", "Value 1" } }).Count());
        }

        [TestMethod]
        public void GetHalfMillionCount()
        {
            GenerateTuples(30, 40, 50, 10);
            Assert.AreEqual(600000, this.engine.GetTupleCount());
            Assert.AreEqual(20000, this.engine.GetTuples(new Dictionary<string, object>() { { "Dimension1", "Value 1" } }).Count());
        }

        private void GenerateTuples(params int[] nvalues)
        {
            int k;
            string[] dimensions = new string[nvalues.Length];

            for (k = 0; k < nvalues.Length; k++)
                dimensions[k] = string.Format("Dimension{0}", k + 1);

            var dict = new Dictionary<string, object>();

            IList<Tuple> tuples = new List<Tuple>();

            k = 0;

            for (var j = 0; j < nvalues[k]; j++)
            {
                var value = string.Format("Value {0}", j + 1);

                Tuple tuple = new Tuple();
                tuple.SetValue(this.engine, dimensions[k], value);
                tuples.Add(tuple);
            }

            IList<Tuple> newtuples;

            for (k = 1; k < nvalues.Length; k++, tuples = newtuples)
            {
                newtuples = new List<Tuple>();
                foreach (var tuple in tuples)
                {
                    for (var j = 0; j < nvalues[k]; j++)
                    {
                        var newtuple = new Tuple(tuple);
                        var value = string.Format("Value {0}", j + 1);
                        newtuple.SetValue(this.engine, dimensions[k], value);
                        newtuples.Add(newtuple);
                    }
                }
            }

            foreach (var t in tuples)
                this.engine.AddTuple(t);
        }
    }
}
