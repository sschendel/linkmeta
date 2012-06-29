using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkMetaDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LinkMeta(string url)
        {
            return Json(LinkMetaUtility.LinkMetaUtility.GetMeta(url), JsonRequestBehavior.AllowGet);
        }

    }
}
