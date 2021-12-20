using ASMTravel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace TestWeb.Controllers
{
    public class HomeController : Controller
    {
        TravelEntities db = new TravelEntities();

        /// <summary>
        /// Home page and show 6 tour 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string search)
        {
            Session["message"] = null;//remive session

           
            return View(db.Tours.OrderByDescending(x=>x.Image.ImgID).Where((x => x.TourStatus == 3 && (x.TourName.StartsWith(search)||search == null))).ToList().ToPagedList(1,6));
        }

        /// <summary>
        /// page about
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }

        
       /// <summary>
       /// page contact
       /// </summary>
       /// <returns></returns>
        public ActionResult Contact_Infor()
        {

            return View();
        }

    }
}