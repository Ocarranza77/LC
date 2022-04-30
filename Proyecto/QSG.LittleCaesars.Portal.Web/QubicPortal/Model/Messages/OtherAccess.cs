using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QubicPortal.Model.Messages
{
    public class OtherAccess
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public bool Granted { get; set; }
    }
}