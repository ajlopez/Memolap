namespace Memolap.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DataSetTests
    {
        [TestMethod]
        public void CreateDataSet()
        {
            DataSet<int> set = new DataSet<int>("Test");

            Assert.AreEqual("Test", set.Name);
            Assert.IsNotNull(set.Dimensions);
            Assert.AreEqual(0, set.Dimensions.Count);
            Assert.IsNull(set.GetDimension("Unknown"));
        }

        [TestMethod]
        public void CreateDimension()
        {
            DataSet<int> set = new DataSet<int>("Data");

            Dimension result = set.CreateDimension("Country");

            Assert.IsNotNull(result);
            Assert.AreEqual("Country", result.Name);
            Assert.AreEqual("Countries", result.SetName);
            Assert.AreEqual(1, set.Dimensions.Count);
            Assert.AreSame(result, set.GetDimension("Country"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfDuplicatedDimension()
        {
            DataSet<int> set = new DataSet<int>("Data");
            set.CreateDimension("Country");
            set.CreateDimension("Country");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfCreateDimensionAfterAddingData()
        {
            DataSet<int> set = new DataSet<int>("Data");
            set.CreateDimension("Country");
            set.AddData(
                new Dictionary<string, object>
                {
                    { "Country", "Canada" }
                },
                100);
            set.CreateDimension("Category");
        }

        [TestMethod]
        public void GetUnknownDimensionAsNull()
        {
            DataSet<int> set = new DataSet<int>("Data");

            Dimension result = set.GetDimension("Country");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetDimension()
        {
            DataSet<int> set = new DataSet<int>("Data");

            Dimension dimension = set.CreateDimension("Country");
            Dimension result = set.GetDimension("Country");

            Assert.IsNotNull(result);
            Assert.AreEqual(dimension, result);
            Assert.AreEqual("Country", result.Name);
        }

        [TestMethod]
        public void GetDimensions()
        {
            DataSet<int> set = new DataSet<int>("Data");
            
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
            DataSet<int> sales = new DataSet<int>("Sales");
            sales.AddData(
                new Dictionary<string, object>
                {
                    { "Country", "Canada" },
                    { "Year", 2012 }
                },
                100);

            TupleObject<int> tuple = sales.Tuples.First();

            Assert.IsNotNull(tuple);
            Assert.AreEqual(2, sales.Dimensions.Count);
            Assert.AreEqual("Canada", tuple.GetValue("Country"));
            Assert.AreEqual(2012, tuple.GetValue("Year"));
            Assert.AreEqual(100, tuple.Data);
        }

        [TestMethod]
        public void GetTupleCount()
        {
            DataSet<int> sales = new DataSet<int>("Sales");

            sales.AddData(
                new Dictionary<string, object>()
                {
                    { "Country", "Argentina" },
                    { "Category", "Beverages" },
                    { "Product", "Beer" }
                },
                100);

            sales.AddData(
                new Dictionary<string, object>()
                {
                    { "Country", "Argentina" },
                    { "Category", "Beverages" },
                    { "Product", "Coke" }
                },
                200);

            Assert.AreEqual(2, sales.Tuples.Count());
        }

        [TestMethod]
        public void GenerateTuplesAndGetCount()
        {
            DataSet<int> sales = new DataSet<int>("Sales");
            
            GenerateTuples(sales, 3, 2);

            Assert.AreEqual(6, sales.Tuples.Count());
        }

        [TestMethod]
        public void GetDimensionOneTuples()
        {
            DataSet<int> sales = new DataSet<int>("Sales");

            GenerateTuples(sales, 3, 4, 5);

            Assert.AreEqual(60, sales.Tuples.Count());
            Assert.AreEqual(20, sales.Query().Where(new Dictionary<string, object>() { { "Dimension1", "Value 1" } }).Tuples.Count());
        }

        [TestMethod]
        public void GetDimensionOneManyTuples()
        {
            DataSet<int> sales = new DataSet<int>("Sales");
            GenerateTuples(sales, 3, 40, 50);

            Assert.AreEqual(6000, sales.Tuples.Count());
            Assert.AreEqual(2000, sales.Query().Where(new Dictionary<string, object>() { { "Dimension1", "Value 1" } }).Tuples.Count());
        }

        [TestMethod]
        public void Query()
        {
            DataSet<int> sales = new DataSet<int>("Sales");

            GenerateTuples(sales, 3, 4, 5, 2);

            var query = sales.Query();

            Assert.IsNotNull(query);
            Assert.AreEqual(120, query.Tuples.Count());
        }

        [TestMethod]
        public void QueryWithWhere()
        {
            DataSet<int> sales = new DataSet<int>("Sales");

            GenerateTuples(sales, 3, 4, 5);

            var query = sales.Query().Where(new Dictionary<string, object>() { { "Dimension1", "Value 1" } });

            Assert.IsNotNull(query);
            Assert.AreEqual(20, query.Tuples.Count());
            Assert.IsTrue(query.Tuples.All(t => t.HasValue("Dimension1", "Value 1")));
        }

        [TestMethod]
        public void QueryWithSkip()
        {
            DataSet<int> sales = new DataSet<int>("Sales");

            GenerateTuples(sales, 3, 4, 5);

            var query = sales.Query().Skip(10);

            Assert.IsNotNull(query);
            Assert.AreEqual(50, query.Tuples.Count());
        }

        [TestMethod]
        public void QueryWithTake()
        {
            DataSet<int> sales = new DataSet<int>("Sales");

            GenerateTuples(sales, 3, 4, 5);

            var query = sales.Query().Skip(10).Take(20);

            Assert.IsNotNull(query);
            Assert.AreEqual(20, query.Tuples.Count());
        }

        private static void GenerateTuples(DataSet<int> set, params int[] nvalues)
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

        private static void GenerateValue(DataSet<int> set, IList<string> dimensions, Dictionary<string, object> values, IList<int> nvalues, int position)
        {
            if (position >= dimensions.Count)
            {
                set.AddData(values, 1);
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
