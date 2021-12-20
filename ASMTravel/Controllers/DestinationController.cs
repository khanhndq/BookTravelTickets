using ASMTravel.Models;
using LoadImage.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace TestWeb.Controllers
{
    public class DestinationController : Controller
    {
        TravelEntities db = new TravelEntities();

        public ActionResult Index()
        {
            return View();
        }

       
        /// <summary>
        /// show list destination
        /// </summary>
        /// <param name="numberTour"></param>
        /// <returns></returns>
        public ActionResult Destination(int ? numberTour, string search)
        {
            //show more 6 tours
            if(numberTour != null)
            {
                TempData["Tour"] = numberTour + 6;
                numberTour = Int32.Parse(TempData["Tour"].ToString());
            }
            else//defaut show 6 tours
            {
                TempData["Tour"] =  6;
                numberTour = Int32.Parse(TempData["Tour"].ToString()) ;
            }

            ViewBag.post = db.Posts.ToList().OrderByDescending(x => x.PostDateTime).Where(x => x.PostStatus == 1).Take(3);

            return View(db.Tours.OrderByDescending(x => x.Image.ImgID).Where(x => x.TourStatus == 3 && (x.TourName.StartsWith(search) || search == null)).ToList().ToPagedList( 1,numberTour?? 6));
        }

        /// <summary>
        /// show tour detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Destination_Detail(string id)
        {
            
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tour tour = db.Tours.Find(id);
                if (tour == null)
                {
                    return HttpNotFound();
                }
                var getTour = new List<Tour>();
                foreach(Tour tour1 in db.Tours.ToList())
                {
                    if (!tour.TourID.Equals(tour1.TourID) && tour1.TourStatus == 3)
                    {
                        getTour.Add(tour1);
                    } 
                }
                ViewBag.tour = getTour.OrderByDescending(x => x.ImgID).Take(3);
            return View(tour);
            }
    }
}