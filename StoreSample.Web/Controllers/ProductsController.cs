using StoreSample.Web.Gateway;
using StoreSample.Web.Models;
using System;
using System.Web.Mvc;

namespace StoreSample.Web.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Details(int id, bool? ordered = false)
        {
            // this is not a nice pattern, but it makes this particular part of the demo
            // nice and easy, which is what is important
            ViewBag.ShowOrderMessage = ordered.GetValueOrDefault(false);

            var product = StockServiceGateway.Instance.GetProduct(id);
            return View(new ProductViewModel()
            {
                Product = product,
                LastCacheRefresh = StockServiceGateway.Instance.LastUpdateTime
            });
        }

        public ActionResult Order(int id, int quantity = 1)
        {
            OrderServiceGateway.SendOrder(id, quantity);
            return RedirectToAction("Details", new { id = id, ordered = true });
        }

        public ActionResult RefreshStock(string returnUrl)
        {
            // we won't use the result, but we'll force the cache to be invalidated
            StockServiceGateway.Instance.InvalidateCache();

            return Redirect(returnUrl);
        }
    }
}