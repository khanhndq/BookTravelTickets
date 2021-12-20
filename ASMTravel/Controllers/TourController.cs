using ASMTravel.Models;
using ASMTravel.ViewModel;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ASMTravel.Controllers
{
    public class TourController : Controller
    {
        TravelEntities db = new TravelEntities();
        /// <summary>
        /// Load list of tour
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string search, int? i)
        {
            var tour = db.Tours.ToList();
            if(search != null)
            {
                Session["Search"] = search;
            }
            else
            {
                Session["Search"] = "";
            }
            
            return View(db.Tours.OrderByDescending(x=>x.Image.ImgID).Where((x => x.TourStatus == 3 && (x.TourName.StartsWith(search)||search == null))).ToList().ToPagedList(i ?? 1,6));
        }

        /// <summary>
        /// check id exists in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CheckTourIDValid(string id)
        {
            if(id != "")
            {
                System.Threading.Thread.Sleep(200);
                var check = db.Tours.Where(m => m.TourID == id).SingleOrDefault();
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
        /// check id tour detail exists in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CheckTourDetailIDValid(string id)
        {
            if (id != "")
            {
                System.Threading.Thread.Sleep(200);
                var check = db.TourDetails.Where(m => m.TourDetailsID == id).SingleOrDefault();
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
        /// create new tour + tour detail + image and add to tables
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var service = db.Services.ToList();//get data of service
            List<ListDropDownService> listSer = new List<ListDropDownService>(); // create new list service
            //loop and add value to list service
            foreach (var item in service)
            {
                var Ser = new ListDropDownService();
                Ser.ServiceID = item.ServiceID;
                Ser.ServiceName = item.ServiceName;
                listSer.Add(Ser);// add value
            }
            //create viewbag store data of services
            ViewBag.service = listSer;
            //get value of places
            var place = db.Places.ToList();
            //store value of place to View bag
            ViewBag.place = place;
            
            return View();
        }
        [HttpPost]
        public ActionResult Create(Tour tour)
        {
            //get value of service in data
            var service = db.Services.ToList();
            //create new list service
            List<ListDropDownService> listSer = new List<ListDropDownService>();
            //loop and add value to list service
            foreach (var item in service)
            {
                var Ser = new ListDropDownService();
                Ser.ServiceID = item.ServiceID;
                Ser.ServiceName = item.ServiceName;
                listSer.Add(Ser);//add value
            }
            //store value service to view bag
            ViewBag.service = listSer;
            //get value of place in data
            var place = db.Places.ToList();
            //store value place to view bag
            ViewBag.place = place;

            //create new object iamge
            Image image = new Image();
            //set image status = 1
            image.status = 1;
            //get name of image with out extension
            string fileName = Path.GetFileNameWithoutExtension(tour.LoadImage.FileName);
            //get extension of image
            string extension = Path.GetExtension(tour.LoadImage.FileName);
            //set filename add 
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            //set image URL to data
            image.ImgURL = "~/Images/" + fileName;
            // get path save file
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            //save iamge to path
            tour.LoadImage.SaveAs(fileName);
            //add value image to data
            db.Images.Add(image);
            //save value to data
            db.SaveChanges();

            //create new OJ detail tour
            TourDetail detail = new TourDetail();
            //asign value to variables 
            detail.TourDetailsID = tour.TourDetailsID;
            detail.ServiceID = tour.TourDetail.ServiceID;
            detail.PlaceID = tour.TourDetail.PlaceID;
            detail.TdStatus = 1;
            detail.TdDescription = tour.TourDetail.TdDescription;
            //add new tour detail
            db.TourDetails.Add(detail);
            //save value to data
            db.SaveChanges();

            //create new OJ tour
            var addtour = new Tour();
            //asign value to variables
            addtour.TourID = tour.TourID;
            addtour.TourDetailsID = detail.TourDetailsID;
            addtour.TourName = tour.TourName;
            addtour.Time = tour.Time;
            addtour.TourAmount = tour.TourAmount;
            addtour.ImgID = image.ImgID;
            addtour.TourStatus = 3;
            //add new value of tour 
            db.Tours.Add(addtour);
            //add value to data and check value successfull
            if (db.SaveChanges() > 0)
            {
              
                return RedirectToAction("Index");//return index of tour
            }
            return View();

        }
        /// <summary>
        /// Find tour with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details(string id)
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

            return View(tour);
        }

        /// <summary>
        /// Find tour want to delete and delete (set value of status in tour = 0 . Not detele tour)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tour = db.Tours.Find(id);

            if (tour == null)
            {
                return HttpNotFound();
            }
            //get image with id
            var image = db.Images.Find(tour.ImgID);
            tour.TourStatus = 0;
            //save modify tour 
            db.Entry(tour).State = EntityState.Modified;
            //save tour to data
            db.SaveChanges();
            //get tour detail with id
            var detail = db.TourDetails.Find(tour.TourDetailsID);
            detail.TdStatus = 0;
            //modify tour detail
            db.Entry(detail).State = EntityState.Modified;
            //save tour detail to data
            db.SaveChanges();
            //get old path image 
            string partImage = Request.MapPath(image.ImgURL);
            image.status = 0;
            image.ImgURL = "";
            //modify value of image
            db.Entry(image).State = EntityState.Modified;
            //save image and check valid 
            if (db.SaveChanges() > 0)
            {   //check file exits if exit so delete file
                if (System.IO.File.Exists(partImage))
                {
                    System.IO.File.Delete(partImage);
                }
                
                return RedirectToAction("Index");
            }
            return View();
        }
        /// <summary>
        /// find tour want to modify
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var tour = db.Tours.Find(id);
            var service = db.Services.ToList();
            List<ListDropDownService> listSer = new List<ListDropDownService>();
            //loop and add value of list service
            foreach (var item in service)
            {
                var Ser = new ListDropDownService();
                Ser.ServiceID = item.ServiceID;
                Ser.ServiceName = item.ServiceName;
                listSer.Add(Ser);
            }
            //create view bag store value of list
            ViewBag.service = listSer;
            //create new list places
            var place = db.Places.ToList();
            //store value of place to view bag
            ViewBag.place = place;
            //get image path
            Session["imagePath"] = tour.Image.ImgURL;
            if (tour == null)
            {
                return HttpNotFound();
            }

            return View(tour);
        }
        /// <summary>
        /// modify values of tour
        /// </summary>
        /// <param name="tour"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Tour tour)
        {
            
            var service = db.Services.ToList();
            List<ListDropDownService> listSer = new List<ListDropDownService>();
            foreach (var item in service)
            {
                var Ser = new ListDropDownService();
                Ser.ServiceID = item.ServiceID;
                Ser.ServiceName = item.ServiceName;
                listSer.Add(Ser);
            }

            ViewBag.service = listSer;

            var place = db.Places.ToList();
            ViewBag.place = place;

            //check valid data
            if (ModelState.IsValid)
            {
                //check image was choose or not
                if (tour.LoadImage != null)
                {

                    //get tour with id
                    var edittour = db.Tours.Find(tour.TourID);
                    //get image with id
                    Image image = db.Images.Find(tour.ImgID);
                    //get name of image with out extension
                    string fileName = Path.GetFileNameWithoutExtension(tour.LoadImage.FileName);
                    //get name of extension
                    string extension = Path.GetExtension(tour.LoadImage.FileName);
                    
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    //set value of path image 
                    image.ImgURL = "~/Images/" + fileName;
                    //get name of path save image
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    //get old path of after path iamge
                    string oldPath = Request.MapPath(Session["imagePath"].ToString());
                    //modify image
                    db.Entry(image).State = EntityState.Modified;
                    //save iamge and check valid data
                    if (db.SaveChanges() > 0)
                    {
                        //valid to save image to path of fileName
                        tour.LoadImage.SaveAs(fileName);
                        //check iamge exit if exists so it's delete
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }

                    }
                    //get tour detail with id 
                    var detail = db.TourDetails.Find(tour.TourDetailsID);
                    detail.TdStatus = 1;
                    detail.TdDescription = tour.TourDetail.TdDescription;
                    //modify values of detail
                    db.Entry(detail).State = EntityState.Modified;
                    //save value to data
                    db.SaveChanges();
                   
                    
                    edittour.TourID = tour.TourID;
                    edittour.TourDetailsID = detail.TourDetailsID;
                    edittour.TourName = tour.TourName;
                    edittour.Time = tour.Time;
                    edittour.TourAmount = tour.TourAmount;
                    edittour.ImgID = image.ImgID;
                    edittour.TourStatus = 3;
                    //modify value of tour
                    db.Entry(edittour).State = EntityState.Modified;
                    //save values to data and check  valid data
                    if (db.SaveChanges() > 0)
                    {
                        
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    //tour va
                    var edittour = db.Tours.Find(tour.TourID);

                    //var tour = tour1;
                    var image = db.Images.Find(tour.ImgID);
                    
                    // set image path 
                    image.ImgURL = Session["imagePath"].ToString();
                    //modify values of iamges
                    db.Entry(image).State = EntityState.Modified;
                    //save iamge to data
                    db.SaveChanges();
                    //get tour detail with id
                    var detail = db.TourDetails.Find(edittour.TourDetailsID);
                    detail.TdDescription = tour.TourDetail.TdDescription;
                    detail.PlaceID = tour.TourDetail.PlaceID;
                    detail.ServiceID = tour.TourDetail.ServiceID;
                    detail.TdStatus = 1;
                    //modify tour detail
                    db.Entry(detail).State = EntityState.Modified;
                    //save values of tour to data
                    db.SaveChanges();


                    

                    edittour.TourID = tour.TourID;
                    edittour.TourDetailsID = detail.TourDetailsID;
                    edittour.TourName = tour.TourName;
                    edittour.Time = tour.Time;
                    edittour.TourAmount = tour.TourAmount;
                    edittour.ImgID = image.ImgID;
                    edittour.TourStatus = 3;
                    //modify values of tour 
                    db.Entry(edittour).State = EntityState.Modified;
                    //save value to data and check valid
                    if (db.SaveChanges() > 0)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return View();
        }

    }
}