using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoadImage.ViewModel
{
    public class CreateTour
    {
        [DisplayName("Tour ID")]
        public string TourID { get; set; }
        [DisplayName("Detail ID")]
        public string TourDetailsID { get; set; }
        [DisplayName("Travel time")]
        public string Time { get; set; }
        [DisplayName("Title Tour")]
        public string TourDescription { get; set; }
        [DisplayName("Price")]
        public Nullable<double> TourAmount { get; set; }
        [DisplayName("Tour Status")]
        public Nullable<int> TourStatus { get; set; }
        [DisplayName("Image ID")]
        public Nullable<int> ImgID { get; set; }
        [DisplayName("Image")]
        public string ImgURL { get; set; }
        [DisplayName("Iamge Status")]
        public Nullable<int> statusImg { get; set; }
        [DisplayName("Service ID")]
        public string ServiceID { get; set; }
        [DataType(DataType.MultilineText)]
        [DisplayName("Description Tour")]
        public string TdDescription { get; set; }
        [DisplayName("Detail Status")]
        public Nullable<int> TdStatus { get; set; }




        public HttpPostedFileBase LoadImage { get; set; }


    }
}