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
    }
}
