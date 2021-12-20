using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ASMTravel.ViewModel
{
    public class ListDropDownService
    {
        [DisplayName("Service ID")]
        public string ServiceID { get; set; }
        [DisplayName("Service Name")]
        public string ServiceName { get; set; }
    }
}