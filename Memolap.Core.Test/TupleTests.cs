﻿namespace Memolap.Core.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TupleTests
    {
        private Engine engine;

        [TestInitialize]
        public void Setup()
        {
            this.engine = new Engine();
        }

        [TestMethod]
        public void HasValue()
        {
            var tuple = new Tuple(this.engine, new Dictionary<string, object>() {
                { "Country", "Argentina" },
                { "Category", "Beverages" },
                { "Product", "Beer" }
            });

            Assert.IsTrue(tuple.HasValue("Country", "Argentina"));
            Assert.IsTrue(tuple.HasValue("Category", "Beverages"));
            Assert.IsTrue(tuple.HasValue("Product", "Beer"));

            Assert.IsFalse(tuple.HasValue("Province", "Buenos Aires"));
        }
    }
}
