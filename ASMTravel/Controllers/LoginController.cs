using ASMTravel.Models;
using ASMTravel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ASMTravel.Controllers
{
    public class LoginController : Controller
    {

        TravelEntities db = new TravelEntities();

        public ActionResult Index()
        {
            if(string.IsNullOrEmpty(Session["message"] as string))
            {
                 Session["message"] = "";//message 
            }
           
            return View();
        }

        /// <summary>
        /// check username and password valid
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        // GET: Login
        public ActionResult Index(Account account)
        {
            string Message = "";
            if (ModelState.IsValid)
            {
                if (checkAccountValid(account))//valid
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                if (!checkAccountValid(account))//invalid
                {
                    Message = "Username or Password incorrect !!!";//message incorrect
                    Session["message"] = Message; //save message to session
                }
            }
            else
            {
                Message = "All fileds Required"; //message error
               Session["message"] = Message;//save message to session
            }
            return RedirectToAction("Index", "Login");
           
           
        }

        /// <summary>
        /// check valid account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool checkAccountValid(Account account)
        {
            bool check = false;
            var staff = db.Staffs.ToList();
            string getPasswrodAccount = GetMD5(account.password.Trim());
            foreach (var item in staff)
            {

                if (getPasswrodAccount.ToLower().Equals(item.password.Trim()) && account.username.Equals(item.username.Trim()))
                {

                    check = true;

                }
            }
            return check;
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
    }
}