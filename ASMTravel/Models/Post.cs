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
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            this.Comments = new HashSet<Comment>();
        }

        [StringLength(10, ErrorMessage = "{0} is too long")]
        [Required(ErrorMessage = "{0} is required")]
        [DisplayName("Post ID")]
        public string PostID { get; set; }

        [StringLength(10, ErrorMessage = "{0} is too long")]
        [Required(ErrorMessage = "{0} is required")]
        [DisplayName("Place ID")]
        public string PlaceID { get; set; }

        [StringLength(255, ErrorMessage = "{0} is too long")]
        [Required(ErrorMessage = "{0} is required")]
        [DisplayName("Post Title")]
        public string PostTitle { get; set; }

      
       
        [DisplayName("Image")]
        public string Image { get; set; }
        

        [DataType(DataType.MultilineText)]
        [StringLength(1000, ErrorMessage = "{0} is too long")]
        [Required(ErrorMessage = "{0} is required")]
        [DisplayName("Description")]
        public string PostDescription { get; set; }

        [DisplayName("Date Create Post")]
        public System.DateTime PostDateTime { get; set; }
        [DisplayName("Status")]
        public Nullable<int> PostStatus { get; set; }
        public HttpPostedFileBase LoadImage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Place Place { get; set; }
        public virtual Place Place1 { get; set; }
    }
}
