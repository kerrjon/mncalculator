using MnCalculator.Models;
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
          if (model.ParentAMonthlyIncomeReceived <= 0 && model.ParentBMonthlyIncomeReceived <= 0)
          {
            ModelState.Clear();
            ModelState.AddModelError(string.Empty, "Oops, you forgot to enter monthly incomes.  At least one parent must have a monthly income.");
            return View("index", model);
          }
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