using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class InitFormRequest
    {
        public int ControlID { get; set; }
        public int CompanyID { get; set; }
    }
}