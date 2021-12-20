using ASMTravel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace ASMTravel.Controllers
{
    public class RegisterCustomerController : Controller
    {

        TravelEntities db = new TravelEntities();
        // GET: RegisterCustomer
        /// <summary>
        /// Show list of Customer regist tour
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string search, int? i)
        {
            if (search != null)
            {
                Session["Search"] = search;
            }
            else
            {
                Session["Search"] = "";
            }
            return View(db.TourRegisters.OrderByDescending(x => x.DateCreate).Where((x => x.Phone.StartsWith(search) || x.Email.StartsWith(search) || x.Name.StartsWith(search) || search == null)).ToList().ToPagedList(i ?? 1, 10));
        }
        /// <summary>
        /// Accept tour of customer registed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Accept(int id)
        {
            var tourRegister = db.TourRegisters.Find(id);
            tourRegister.status = 1; //status accept
            db.Entry(tourRegister).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index",new { search = Session["Search"] });
        }
        /// <summary>
        /// Cancel tour customer registed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            var tourRegister = db.TourRegisters.Find(id);
            tourRegister.status = 0;// status delete
            db.Entry(tourRegister).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { search = Session["Search"] });
        }
        /// <summary>
        /// Add information of customer to data
        /// </summary>
        /// <param name="tourRegister"></param>
        /// <returns></returns>
        public ActionResult Register(TourRegister tourRegister)
        {
            //get now time
            tourRegister.DateCreate = DateTime.Now;
            tourRegister.status = 2;//set status wating process
            //add tour register to data
            db.TourRegisters.Add(tourRegister);
            //save data and check valid data
            if (db.SaveChanges() > 0)
            {

                return RedirectToAction("Destination_Detail","Destination", new { id = tourRegister.TourID.Trim()});
            }
            return View();
        }
    }
}