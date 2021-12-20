using ASMTravel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace ASMTravel.Controllers
{
    public class StaffController : Controller
    {

        TravelEntities db = new TravelEntities();

        // GET: Staff
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
            return View(db.Staffs.OrderByDescending(x=>x.StaffID).Where((x => x.StStatus != 0 && (x.username.StartsWith(search) || x.Fullname.StartsWith(search) || search == null))).ToList().ToPagedList(i ?? 1, 10));
        }

        /// <summary>
        /// change password to MD5
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public String GetMD5(string txt)
        {
            String str = "";
            Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(txt);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);
            foreach (Byte b in buffer)
            {
                str += b.ToString("X2");
            }
            return str;
        }


        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Staff staff)
        {
            staff.password = GetMD5(staff.password.Trim()).ToLower();
            staff.StStatus = 1; //status wating process
            staff.Role = 2;
            //add data of comment 
            db.Staffs.Add(staff);
            //save data and check valid 
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("Index", "Staff", new { id = staff.StaffID.Trim() });
            }
            
            return View();
        }
        /// <summary>
        /// check id staff exists in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CheckStaffIDValid(string id)
        {
            if (id != "")
            {
                System.Threading.Thread.Sleep(200);
                var check = db.Staffs.Where(m => m.StaffID == id).SingleOrDefault();
                if (check != null)
                {
                    return Json(1);
                }
                else
                {
                    return Json(0);
                }
            }
            return Json(0);
        }
        /// <summary>
        /// check username exists in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CheckUsernameValid(string id)
        {
            if (id != "")
            {
                System.Threading.Thread.Sleep(200);
                var check = db.Staffs.Where(m => m.username == id).SingleOrDefault();
                if (check != null)
                {
                    return Json(1);
                }
                else
                {
                    return Json(0);
                }
            }
            return Json(0);
        }


    }
}