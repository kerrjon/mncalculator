using MnCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MnCalculator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new ChildSupportForm();
            return View(model);
        }

        [HttpPost]
        public ActionResult Result(ChildSupportForm model)
        {
            var calculatorResult = CalculatorResult.Create(model);
            return View(calculatorResult);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}