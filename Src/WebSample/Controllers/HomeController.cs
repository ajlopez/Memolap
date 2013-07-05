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
        private DataBank bank;

        public HomeController()
            : this(Domain.Current.DataBank)
        {
        }

        public HomeController(DataBank bank)
        {
            this.bank = bank;
        }

        public ActionResult Index()
        {
            var model = this.bank.Dimensions.Select(d => new DimensionModel() { Name = d.Name, SetName = d.SetName }).ToList();
            return View(model);
        }
    }
}
