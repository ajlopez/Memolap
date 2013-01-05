namespace WebSample.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebSample.Controllers;
    using WebSample.Models;

    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        [DeploymentItem("WebSampleTestFiles", "WebSampleTestFiles")]
        public void GetIndex()
        {
            Domain domain = new Domain();
            domain.InitializeFromFolder("WebSampleTestFikes");

            HomeController controller = new HomeController();

            var result = controller.Index();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var vresult = (ViewResult)result;
            Assert.IsNotNull(vresult.Model);
            Assert.IsInstanceOfType(vresult.Model, typeof(IList<DimensionModel>));

            var model = (IList<DimensionModel>)vresult.Model;

            Assert.AreEqual(2, model.Count);
            Assert.IsTrue(model.Any(m => m.SetName == "Countries"));
            Assert.IsTrue(model.Any(m => m.SetName == "Categories"));
        }
    }
}
