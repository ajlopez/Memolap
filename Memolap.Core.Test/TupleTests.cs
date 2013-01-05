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

        [TestMethod]
        public void SetValue()
        {
            var tuple = new Tuple();
            tuple.SetValue(this.engine, "Country", "Argentina");
            Assert.IsTrue(tuple.HasValue("Country", "Argentina"));
            tuple.SetValue(this.engine, "Country", "Uruguay");
            Assert.IsTrue(tuple.HasValue("Country", "Uruguay"));
            Assert.IsFalse(tuple.HasValue("Country", "Argentina"));
        }

        [TestMethod]
        public void CloneTuple()
        {
            var tuple = new Tuple(this.engine, new Dictionary<string, object>() {
                { "Country", "Argentina" },
                { "Category", "Beverages" },
                { "Product", "Beer" }
            });
            var newtuple = new Tuple(tuple);
            Assert.IsTrue(newtuple.HasValue("Country", "Argentina"));
            Assert.IsTrue(newtuple.HasValue("Category", "Beverages"));
            Assert.IsTrue(newtuple.HasValue("Product", "Beer"));
            newtuple.SetValue(this.engine, "Country", "Uruguay");
            Assert.IsFalse(newtuple.HasValue("Country", "Argentina"));
            Assert.IsTrue(tuple.HasValue("Country", "Argentina"));
            Assert.IsTrue(newtuple.HasValue("Country", "Uruguay"));
            Assert.IsTrue(newtuple.HasValue("Category", "Beverages"));
            Assert.IsTrue(newtuple.HasValue("Product", "Beer"));
        }

        [TestMethod]
        public void Match()
        {
            var tuple = new Tuple(this.engine, new Dictionary<string, object>() {
                { "Country", "Argentina" },
                { "Category", "Beverages" },
                { "Product", "Beer" }
            });

            Assert.IsTrue(tuple.Match(new Dictionary<string, object>() { 
                { "Country", "Argentina" }
            }));

            Assert.IsTrue(tuple.Match(new Dictionary<string, object>() { 
                { "Country", "Argentina" },
                { "Category", "Beverages" }
            }));

            Assert.IsTrue(tuple.Match(new Dictionary<string, object>() { 
                { "Product", "Beer" },
                { "Category", "Beverages" }
            }));

            Assert.IsFalse(tuple.Match(new Dictionary<string, object>() { 
                { "Country", "Chile" }
            }));

            Assert.IsFalse(tuple.Match(new Dictionary<string, object>() { 
                { "Country", "Chile" },
                { "Category", "Beverages" }
            }));

            Assert.IsFalse(tuple.Match(new Dictionary<string, object>() { 
                { "Product", "Coffee" },
                { "Category", "Beverages" }
            }));
        }

        [TestMethod]
        public void MatchWithNull()
        {
            var tuple = new Tuple(this.engine, new Dictionary<string, object>() {
                { "Country", "Argentina" },
                { "Category", "Beverages" },
                { "Product", "Beer" }
            });

            Assert.IsTrue(tuple.Match(new Dictionary<string, object>() { 
                { "Country", null }
            }));

            Assert.IsTrue(tuple.Match(new Dictionary<string, object>() { 
                { "Country", "Argentina" },
                { "Category", null }
            }));

            Assert.IsTrue(tuple.Match(new Dictionary<string, object>() { 
                { "Product", null },
                { "Category", "Beverages" }
            }));

            Assert.IsFalse(tuple.Match(new Dictionary<string, object>() { 
                { "Country", "Chile" },
                { "Category", null }
            }));

            Assert.IsFalse(tuple.Match(new Dictionary<string, object>() { 
                { "Product", "Coffee" },
                { "Category", null }
            }));
        }
    }
}
