//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASMTravel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Place
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Place()
        {
            this.Posts = new HashSet<Post>();
            this.Posts1 = new HashSet<Post>();
            this.TourDetails = new HashSet<TourDetail>();
        }

        [DisplayName("Place ID")]
        public string PlaceID { get; set; }
        [DisplayName("Location ID")]
        public string LocationID { get; set; }
        [DisplayName("Hotel ID")]
        public string HotelID { get; set; }
        [DisplayName("Restaurant ID")]
        public string RestaurantID { get; set; }
        [DisplayName("Place Name")]
        public string PlaceName { get; set; }
        [DisplayName("Place Address")]
        public string PlaceAddress { get; set; }
        [DisplayName("Place Description")]
        public string PlaceDescription { get; set; }
        [DisplayName("Status")]
        public Nullable<int> PlaceStatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TourDetail> TourDetails { get; set; }
    }
}