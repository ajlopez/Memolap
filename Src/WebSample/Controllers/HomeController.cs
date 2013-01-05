namespace WebSample.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Memolap.Core;
using WebSample.Models;

    public class HomeController : Controller
    {
        private Engine engine;

        public HomeController()
            : this(Domain.Current.Engine)
        {
        }

        public HomeController(Engine engine)
        {
            this.engine = engine;
        }

        public ActionResult Index()
        {
            var model = this.engine.GetDimensions().Select(d => new DimensionModel() { Name = d.Name, SetName = d.SetName }).ToList();
            return View(model);
        }
    }
}
