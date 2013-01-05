namespace Memolap.Core.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
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
    }
}
