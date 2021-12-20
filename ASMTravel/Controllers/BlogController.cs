using ASMTravel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace TestWeb.Controllers.Content
{
    public class BlogController : Controller
    {
        TravelEntities db = new TravelEntities();       

        /// <summary>
        /// Show list post in Blog page
        /// </summary>
        /// <returns></returns>
        public ActionResult Blog(string search, int? i)
        {
            //var post = db.Posts.ToList();
            //var getPost = new List<Post>();
            //foreach (var item in post)
            //{
            //    if(item.PostStatus == 3)
            //    {
            //        getPost.Add(item);
            //    }
            //}
            return View(db.Posts.OrderByDescending(x=>x.PostDateTime).Where(x => x.PostStatus == 1 && (x.PostTitle.StartsWith(search) || search == null)).ToList().ToPagedList(i ?? 1, 3));
        }
        /// <summary>
        /// Show information detail a blog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Blog_Detail(string id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.comment = db.Comments.OrderByDescending(x => x.CommentID).ToList();
        
            return View(post);
        }
    }
}