namespace WebSample.Tests.Controllers
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebSample.Controllers;
    using System.Web.Mvc;

    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void GetIndex()
        {
            HomeController controller = new HomeController();

            var result = controller.Index();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var vresult = (ViewResult)result;
            Assert.IsNull(vresult.Model);
        }
    }
}
