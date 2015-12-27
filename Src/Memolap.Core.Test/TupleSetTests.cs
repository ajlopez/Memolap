namespace Memolap.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TupleSetTests
    {
        [TestMethod]
        public void CreateTupleSet()
        {
            TupleSet<int> set = new TupleSet<int>("Test");

            Assert.AreEqual("Test", set.Name);
            Assert.IsNotNull(set.Dimensions);
            Assert.AreEqual(0, set.Dimensions.Count);
            Assert.AreEqual(-1, set.GetDimensionOffset("Unknown"));
        }

        [TestMethod]
        public void CreateDimension()
        {
            TupleSet<int> set = new TupleSet<int>("Data");

            Dimension result = set.CreateDimension("Country");

            Assert.IsNotNull(result);
            Assert.AreEqual("Country", result.Name);
            Assert.AreEqual("Countries", result.SetName);
            Assert.AreEqual(1, set.Dimensions.Count);
            Assert.AreEqual(0, set.GetDimensionOffset("Country"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfDuplicatedDimension()
        {
            TupleSet<int> set = new TupleSet<int>("Data");
            set.CreateDimension("Country");
            set.CreateDimension("Country");
        }

        [TestMethod]
        public void GetUnknownDimensionAsNull()
        {
            TupleSet<int> set = new TupleSet<int>("Data");

            Dimension result = set.GetDimension("Country");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetDimension()
        {
            TupleSet<int> set = new TupleSet<int>("Data");

            Dimension dimension = set.CreateDimension("Country");
            Dimension result = set.GetDimension("Country");

            Assert.IsNotNull(result);
            Assert.AreEqual(dimension, result);
            Assert.AreEqual("Country", result.Name);
        }

        [TestMethod]
        public void GetDimensions()
        {
            TupleSet<int> set = new TupleSet<int>("Data");
            
            set.CreateDimension("Country");
            set.CreateDimension("Product");
            set.CreateDimension("Category");

            var result = set.Dimensions;

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
        public void CreateTuple()
        {
            TupleSet<int> sales = new TupleSet<int>("Sales");

            TupleObject<int> tuple = sales.CreateTuple(
                new Dictionary<string, object>
                {
                    { "Country", "Canada" },
                    { "Year", 2012 }
                }, 
                100);

            Assert.IsNotNull(tuple);
            Assert.AreEqual(2, sales.Dimensions.Count);
            Assert.AreEqual("Canada", tuple.GetValue("Country"));
            Assert.AreEqual(2012, tuple.GetValue("Year"));
            Assert.AreEqual(100, tuple.Data);
        }

        [TestMethod]
        public void GetTupleCount()
        {
            TupleSet<int> sales = new TupleSet<int>("Sales");

            sales.CreateTuple(
                new Dictionary<string, object>()
                {
                    { "Country", "Argentina" },
                    { "Category", "Beverages" },
                    { "Product", "Beer" }
                },
                100);

            sales.CreateTuple(
                new Dictionary<string, object>()
                {
                    { "Country", "Argentina" },
                    { "Category", "Beverages" },
                    { "Product", "Coke" }
                },
                200);

            Assert.AreEqual(2, sales.GetTupleCount());
        }

        [TestMethod]
        public void GenerateTuplesAndGetCount()
        {
            TupleSet<int> sales = new TupleSet<int>("Sales");
            
            GenerateTuples(sales, 3, 2);

            Assert.AreEqual(6, sales.GetTupleCount());
        }

        [TestMethod]
        public void GetDimensionOneTuples()
        {
            TupleSet<int> sales = new TupleSet<int>("Sales");

            GenerateTuples(sales, 3, 4, 5);

            Assert.AreEqual(60, sales.GetTupleCount());
            Assert.AreEqual(20, sales.GetTuples(new Dictionary<string, object>() { { "Dimension1", "Value 1" } }).Count());
        }

        [TestMethod]
        public void GetDimensionOneManyTuples()
        {
            TupleSet<int> sales = new TupleSet<int>("Sales");

            GenerateTuples(sales, 3, 40, 50);

            Assert.AreEqual(6000, sales.GetTupleCount());
            Assert.AreEqual(2000, sales.GetTuples(new Dictionary<string, object>() { { "Dimension1", "Value 1" } }).Count());
        }

        [TestMethod]
        public void GetTuplesValues()
        {
            TupleSet<int> sales = new TupleSet<int>("Sales");

            GenerateTuples(sales, 3, 4, 5, 2);

            var values = sales.GetTuplesValues(new Dictionary<string, object>() { { "Dimension1", "Value 1" } }, "Dimension2");
            Assert.IsNotNull(values);
            Assert.AreEqual(4, values.Count);
            Assert.IsTrue(values.Any(val => val.Equals("Value 1")));
            Assert.IsTrue(values.Any(val => val.Equals("Value 2")));
            Assert.IsTrue(values.Any(val => val.Equals("Value 3")));
            Assert.IsTrue(values.Any(val => val.Equals("Value 4")));
        }

        [TestMethod]
        public void Query()
        {
            TupleSet<int> sales = new TupleSet<int>("Sales");

            GenerateTuples(sales, 3, 4, 5, 2);

            var query = sales.Query();

            Assert.IsNotNull(query);
            Assert.AreEqual(120, query.Tuples.Count());
        }

        [TestMethod]
        public void WhereQuery()
        {
            TupleSet<int> sales = new TupleSet<int>("Sales");

            GenerateTuples(sales, 3, 4, 5);

            var query = sales.Query().Where(new Dictionary<string, object>() { { "Dimension1", "Value 1" } });

            Assert.IsNotNull(query);
            Assert.AreEqual(20, query.Tuples.Count());
            Assert.IsTrue(query.Tuples.All(t => t.HasValue("Dimension1", "Value 1")));
        }

        private static void GenerateTuples(TupleSet<int> set, params int[] nvalues)
        {
            int k;
            string[] dimensions = new string[nvalues.Length];

            for (k = 0; k < nvalues.Length; k++)
            {
                dimensions[k] = string.Format("Dimension{0}", k + 1);
                set.CreateDimension(dimensions[k]);
            }

            var dict = new Dictionary<string, object>();

            GenerateValue(set, dimensions, dict, nvalues, 0);
        }

        private static void GenerateValue(TupleSet<int> set, IList<string> dimensions, Dictionary<string, object> values, IList<int> nvalues, int position)
        {
            if (position >= dimensions.Count)
            {
                set.CreateTuple(values, 1);
                return;
            }

            for (int k = 0; k < nvalues[position]; k++)
            {
                string value = string.Format("Value {0}", k + 1);
                values[dimensions[position]] = value;

                GenerateValue(set, dimensions, values, nvalues, position + 1);
            }
        }

        private class Counter
        {
            public int Count { get; set; }
        }
    }
}
