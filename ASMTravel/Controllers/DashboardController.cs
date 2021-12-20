using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASMTravel.Controllers
{
    public class DashboardController : Controller
    {
        /// <summary>
        /// page dashboard
        /// </summary>
        /// <returns></returns>
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}