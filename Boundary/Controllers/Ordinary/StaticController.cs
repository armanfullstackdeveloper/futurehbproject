using System.Web.Mvc;

namespace Boundary.Controllers.Ordinary
{
    public class StaticController : Controller
    {
         public ActionResult UserGuid()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Law()
        {
            return View();
        }

        public ActionResult Complaints()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

    }
}