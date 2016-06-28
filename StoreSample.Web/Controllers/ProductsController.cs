using StoreSample.Web.Gateway;
using StoreSample.Web.Models;
using System;
using System.Web.Mvc;

namespace StoreSample.Web.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Details(int id)
        {
            var product = StockServiceGateway.Instance.GetProduct(id);
            return View(new ProductViewModel()
            {
                Product = product,
                LastCacheRefresh = StockServiceGateway.Instance.LastUpdateTime
            });
        }

        public ActionResult Order(int id)
        {

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult RefreshStock(string returnUrl)
        {
            // we won't use the result, but we'll force the cache to be invalidated
            StockServiceGateway.Instance.InvalidateCache();

            return Redirect(returnUrl);
        }
    }
}