namespace WebSample.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DomainTests
    {
        [TestMethod]
        [DeploymentItem("WebSampleTestFiles", "WebSampleTestFiles")]
        public void LoadFromFolder()
        {
            Domain domain = new Domain();
            domain.InitializeFromFolder("WebSampleTestFiles");

            var dimension = domain.DataBank.GetDimension("Category");

            Assert.IsNotNull(dimension);

            var values = dimension.GetValues();

            Assert.IsNotNull(values);
            Assert.AreEqual(8, values.Count);

            dimension = domain.DataBank.GetDimension("Country");

            Assert.IsNotNull(dimension);

            values = dimension.GetValues();

            Assert.IsNotNull(values);
            Assert.AreEqual(8, values.Count);
        }
    }
}
