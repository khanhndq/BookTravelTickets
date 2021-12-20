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
    public class CommentController : Controller
    {
        TravelEntities db = new TravelEntities();
        
        /// <summary>
        /// Show list comment 
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
            return View(db.Comments.OrderByDescending(x => x.CommentID).Where(x=> x.CmEmail.StartsWith(search) || search == null).ToList().ToPagedList(i ?? 1, 10));
        }
        /// <summary>
        /// Send comment to data and wating process
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendComment(Comment comment)
        {
            comment.CmStatus = 2; //status wating process
            //add data of comment 
            db.Comments.Add(comment);
            //save data and check valid 
            if(db.SaveChanges() > 0)
            {

                return RedirectToAction("Blog_Detail","Blog", new { id = comment.PostID.Trim() });
            }
            return View();
        }
        /// <summary>
        /// Accept comment wating process
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Accept(int id)
        {
            var comment = db.Comments.Find(id);

            comment.CmStatus = 1; //Process status
            //modify Status of comment
            db.Entry(comment).State = EntityState.Modified;
            //save data 
            db.SaveChanges();
            
            return RedirectToAction("Index", new { search = Session["Search"] });
        }
        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            //get comment 
            var comment = db.Comments.Find(id);
            comment.CmStatus = 0;//delete status
            //modify status of comment
            db.Entry(comment).State = EntityState.Modified;
            //save data
            db.SaveChanges();
            return RedirectToAction("Index", new { search = Session["Search"] });
        }

    }
}