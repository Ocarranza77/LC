using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class Form
    {
        public bool Read { get; set; }

        public bool Write { get; set; }

        public bool Print { get; set; }

        public bool Delete { get; set; }

        public bool Export { get; set; }

        public List<OtherAccess> Others { get; set; }
    }
}