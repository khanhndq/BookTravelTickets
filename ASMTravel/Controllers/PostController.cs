using ASMTravel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace ASMTravel.Controllers
{
    public class PostController : Controller
    {

        TravelEntities db = new TravelEntities();
        /// <summary>
        /// Show list post
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string search, int? i)
        {
            //var post = db.Posts.ToList();
            //List<Post> postValid = new List<Post>();
            //foreach (var item in post)
            //{
            //    if(item.PostStatus == 1)
            //    {
            //        postValid.Add(item);
            //    }
            //}
            if (search != null)
            {
                Session["Search"] = search;
            }
            else
            {
                Session["Search"] = "";
            }
            return View(db.Posts.OrderByDescending(x => x.PostDateTime).Where((x => x.PostStatus == 1 && (x.PostTitle.StartsWith(search) || search == null))).ToList().ToPagedList(i ?? 1, 6));
        }

        /// <summary>
        /// check id post exists in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CheckPostIDValid(string id)
        {
            if (id != "")
            {
                System.Threading.Thread.Sleep(200);
                var check = db.Posts.Where(m => m.PostID == id).SingleOrDefault();
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
        /// Create new post
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var place = db.Places.ToList();
            ViewBag.place = place;
            return View();
        }
        /// <summary>
        /// Create new post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Post post)
        {
            //get list places
            var place = db.Places.ToList();
            //store place to view bag
            ViewBag.place = place;

            post.PostStatus = 1;
            //get name of image with out extension
            string fileName = Path.GetFileNameWithoutExtension(post.LoadImage.FileName);
            //get name of extension
            string extension = Path.GetExtension(post.LoadImage.FileName);
           
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            //set image of post
            post.Image = "~/ImgPost/" + fileName;
            //get name path store image
            fileName = Path.Combine(Server.MapPath("~/ImgPost/"), fileName);
            
            post.PostDateTime = DateTime.Now;
            //save image to path
            post.LoadImage.SaveAs(fileName);
            //add new post
            db.Posts.Add(post);
            //save post to data
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get post with id
            var post = db.Posts.Find(id);
            //get all place form data
            var place = db.Places.ToList();
            //store palce to view bag
            ViewBag.place = place;
            //get URl of image
            Session["imagePath"] = post.Image;
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }
        /// <summary>
        /// edit post 
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Post post)
        {
            //get all place form data
            var place = db.Places.ToList();
            //store all place to view bag
            ViewBag.place = place;
            //check valid data
            if (ModelState.IsValid)
            {
                //check have image or not
                if (post.LoadImage != null)
                {
                    //get name of image with out extension
                    string fileName = Path.GetFileNameWithoutExtension(post.LoadImage.FileName);
                    //get name of extension
                    string extension = Path.GetExtension(post.LoadImage.FileName);
                    
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    //set URL of imae
                    post.Image = "~/ImgPost/" + fileName;
                    //set path of file store image
                    fileName = Path.Combine(Server.MapPath("~/ImgPost/"), fileName);
                    // get old path image
                    string oldPath = Request.MapPath(Session["imagePath"].ToString());
                    //modify post
                    db.Entry(post).State = EntityState.Modified;
                    //save post to data and check valid
                    if (db.SaveChanges() > 0)
                    {
                        //save iamge to path
                        post.LoadImage.SaveAs(fileName);
                        //check exit if exists so it's delete
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                        return RedirectToAction("Index");
                    }
                }
                else//if not choose image
                {
                    post.PostStatus = 1;
                    //set RUL image
                    post.Image = Session["imagePath"].ToString();
                    //modify post
                    db.Entry(post).State = EntityState.Modified;
                    //save post to data and check valid
                    if (db.SaveChanges() > 0)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return View();
        }

        /// <summary>
        /// delete post (set status of post = 0 . Not delete post)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = db.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }
          
            post.PostStatus = 0;
            //get path of image
            string partImage = Request.MapPath(post.Image);
            //modify post 
            db.Entry(post).State = EntityState.Modified;
            //save post to data and check valid
            if (db.SaveChanges() > 0)
            {
                //if iamge exists so it's delete
                if (System.IO.File.Exists(partImage))
                {
                    System.IO.File.Delete(partImage);
                }
                return RedirectToAction("Index");
            }
            return View();

        }
        /// <summary>
        /// show information of a post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = db.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }
    }
}