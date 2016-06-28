using StoreSample.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StoreSample.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var products = Gateway.StockServiceGateway.Instance.GetAllProducts();
            return View(products);
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