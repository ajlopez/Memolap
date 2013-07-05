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
        private TupleSet set;

        public HomeController()
            : this(Domain.Current.TupleSet)
        {
        }

        public HomeController(TupleSet set)
        {
            this.set = set;
        }

        public ActionResult Index()
        {
            var model = this.set.Dimensions.Select(d => new DimensionModel() { Name = d.Name, SetName = d.SetName }).ToList();
            return View(model);
        }
    }
}
